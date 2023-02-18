using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using RingFunctionAppHost.Logics;
using RingFunctionAppHost.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RingFunctionAppHost;

public class SnapshotCollectorFunction
{
    private readonly RingSession _ringSession;
    private readonly HttpClient _httpClient;
    private readonly ILogger<SnapshotCollectorFunction> _logger;

    public SnapshotCollectorFunction(RingSession ringSession
        , HttpClient httpClient
        , ILogger<SnapshotCollectorFunction> logger)
    {
        _ringSession = ringSession;
        _httpClient = httpClient;
        _logger = logger;
    }

    [FunctionName(nameof(SnapshotCollectorFunction))]
    public async Task Run(
        [TimerTrigger("0 15 15 * * *"
#if DEBUG
        , RunOnStartup = true
#endif
        )] TimerInfo myTimer,
        [Blob("snapshots",FileAccess.Read,Connection = "StorageConnectionString")]
        BlobContainerClient blobContainerClient,
        [Table("Configuration", Connection = "StorageConnectionString")] TableClient tableClient)
    {
        var config = await tableClient.GetEntityAsync<Config>(nameof(SnapshotCollectorFunction), nameof(Config));
        await _ringSession.InitializeAsync(config.Value.RefreshToken);

        try
        {
            var span = TimeSpan.FromHours(3);

            List<OrchestratorTimelineResponse> responses = new();
            var currentDate = config.Value.LastCollectedDateTime;

            while (currentDate <= DateTimeOffset.UtcNow)
            {
                OrchestratorTimelineResponse response = default!;
                do
                {
                    response = await _ringSession.GetOrchestratorTimelineAsync(
                    config.Value.DoorbotId
                    , currentDate
                    , currentDate.Add(span)
                    , isOderByAsc: true
                    , paginationKey: response?.pagination_key);
                    responses.Add(response);
                } while (response.pagination_key != null);
                currentDate = currentDate.Add(span);
            }

            var footages = responses.SelectMany(r => r.supplemental_data.footages).ToList();

            if (footages.Any())
            {
                foreach (var footage in footages)
                {
                    var startTime = DateTimeOffset.Parse(footage.start_time);
                    var endTime = DateTimeOffset.Parse(footage.end_time);
                    var path = $"{startTime:yyyy/MM/dd/yyyy-MM-ddTHH:mm:ssK}-{endTime:yyyy-MM-ddTHH:mm:ssK}.mp4";

                    var blobClient = blobContainerClient.GetBlobClient(path);

                    if (!await blobClient.ExistsAsync())
                    {
                        using var fileStream = await _httpClient.GetStreamAsync(footage.url);
                        await blobClient.UploadAsync(fileStream);
                    }
                }
                config.Value.LastCollectedDateTime = footages.Max(f => DateTimeOffset.Parse(f.end_time));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Run));
        }
        finally
        {
            config.Value.RefreshToken = _ringSession.CurrentRefreshToken;
            await tableClient.UpdateEntityAsync(config.Value, ETag.All);
        }
    }

    public class Config : ITableEntity
    {
        public int DoorbotId { get; set; }
        public string RefreshToken { get; set; } = default!;
        public DateTimeOffset LastCollectedDateTime { get; set; }

        public string PartitionKey { get; set; } = default!;
        public string RowKey { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}

using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RingFunctionAppHost.Logics;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RingFunctionAppHost
{
    public class LocalTriggerFunction(RingSession ringSession
        , HttpClient httpClient
        , ILogger<SnapshotCollectorFunction> logger1
        , IOptionsMonitor<ExecutionContextOptions> executionContextOptions
        , IConfiguration configuration
        , ILogger<MakeDailyTimelapseFunction> logger2)
    {
        private readonly SnapshotCollectorFunction _snapshotCollectorFunction = new(ringSession, httpClient, logger1);
        private readonly MakeDailyTimelapseFunction _makeDailyTimelapseFunction = new(executionContextOptions, configuration, logger2);

        [FunctionName(nameof(LocalTriggerFunction))]
        public async Task<IActionResult> Run(
#pragma warning disable IDE0060
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        [Table("Configuration", Connection = "StorageConnectionString")] TableClient tableClient,
#pragma warning restore IDE0060
        [Blob("snapshots",FileAccess.Read,Connection = "StorageConnectionString")]
        BlobContainerClient snapshotsBlobContainerClient,
        [Blob("timelapses",FileAccess.Read,Connection = "StorageConnectionString")]
        BlobContainerClient timelapsesBlobContainerClient)
        {
            //await _snapshotCollectorFunction.Run(default!, snapshotsBlobContainerClient, tableClient);
            //await _makeDailyTimelapseFunction.Run(default!, snapshotsBlobContainerClient, timelapsesBlobContainerClient);
            for(int i = -24; i < 0; i++)
            {
                try
                {
                    _makeDailyTimelapseFunction.OffsetDays = i;
                    await _makeDailyTimelapseFunction.Run(default!, snapshotsBlobContainerClient, timelapsesBlobContainerClient);

                }
                catch (Exception)
                {

                }
            }
            return new OkResult();
        }
    }
}

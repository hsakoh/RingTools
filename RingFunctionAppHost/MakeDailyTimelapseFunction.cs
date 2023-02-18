using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace RingFunctionAppHost;

public class MakeDailyTimelapseFunction
{
    private readonly ILogger<MakeDailyTimelapseFunction> _logger;
    private readonly string ffmpgPath;
    private readonly TimeSpan timeZoneOffset;
    private readonly int offsetDays;

    public MakeDailyTimelapseFunction(
        IOptionsMonitor<ExecutionContextOptions> executionContextOptions
        , IConfiguration configuration
        , ILogger<MakeDailyTimelapseFunction> logger)
    {
        _logger = logger;
        ffmpgPath = Path.Combine(executionContextOptions.CurrentValue.AppDirectory, "ffmpeg", "ffmpeg.exe");
        timeZoneOffset = configuration.GetValue<TimeSpan>("TimeZoneOffset");
        offsetDays = configuration.GetValue<int>("OffsetDays");
    }

    [FunctionName(nameof(MakeDailyTimelapseFunction))]
    public async Task Run(
        [TimerTrigger("0 30 15 * * *"
#if DEBUG
        , RunOnStartup = true
#endif
        )] TimerInfo myTimer,
        [Blob("snapshots",FileAccess.Read,Connection = "StorageConnectionString")]
        BlobContainerClient snapshotsBlobContainerClient,
        [Blob("timelapses",FileAccess.Read,Connection = "StorageConnectionString")]
        BlobContainerClient timelapsesBlobContainerClient)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("D"));
        Directory.CreateDirectory(tempDir);
        _logger.LogInformation("TempDir:{TempDir}", tempDir);

        var targetDate = DateTimeOffset.UtcNow.Add(timeZoneOffset).Date.AddDays(-1).AddDays(offsetDays);
        var start = new DateTimeOffset(targetDate, timeZoneOffset);
        var end = new DateTimeOffset(targetDate.AddDays(1), timeZoneOffset);

        _logger.LogInformation("Target:{Target}", targetDate);
        _logger.LogInformation("Start:{Start}", start);
        _logger.LogInformation("End:{End}", end);

        var inputListFilePath = await DownloadSnapshotsAndMakeInputFileListAsync(tempDir, start, end, snapshotsBlobContainerClient);

        var outputPath = GenerateTimelapse(tempDir, inputListFilePath);

        await UploadTimelapseAsync(timelapsesBlobContainerClient, targetDate, outputPath);

        Directory.Delete(tempDir, true);
    }

    private async Task<string> DownloadSnapshotsAndMakeInputFileListAsync(
        string tempDir
        , DateTimeOffset start, DateTimeOffset end
        , BlobContainerClient snapshotsBlobContainerClient)
    {
        var inputListFilePath = Path.Combine(tempDir, "input.txt");
        using var inputListFileWriter = new StreamWriter(inputListFilePath, false);
        int fileIndex = 0;
        var current = start;
        while (current < end)
        {
            var resultSegment = snapshotsBlobContainerClient.GetBlobsAsync(prefix: $"{current.ToUniversalTime():yyyy/MM/dd/yyyy-MM-ddTHH}").AsPages();
            List<string> blobs = new();
            await foreach (Page<BlobItem> blobPage in resultSegment)
            {
                blobs.AddRange(blobPage.Values.Select(x => x.Name));
            }

            foreach (var blob in blobs.OrderBy(s => s))
            {
                var localPath = Path.Combine(tempDir, $"{fileIndex++}.mp4");
                var blobClient = snapshotsBlobContainerClient.GetBlobClient(blob);
                await blobClient.DownloadToAsync(localPath);
                inputListFileWriter.WriteLine($"file {localPath.Replace("\\", "\\\\")}");

                _logger.LogInformation($"{{Index}},{{BlobPath}},{{LocalPath}}", fileIndex, blob, localPath);
            }
            current = current.AddHours(1);
        }

        if (fileIndex == 0)
        {
            throw new Exception($"target file nothing {start},{end}");
        }
        return inputListFilePath;
    }

    private string GenerateTimelapse(string tempDir, string inputListFilePath)
    {
        var outputPath = Path.Combine(tempDir, "output.mp4");
        using var process = new Process();
        process.StartInfo = new()
        {
            FileName = ffmpgPath,
            Arguments = $"-safe 0 -f concat -i \"{inputListFilePath}\" -c copy \"{outputPath}\"",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        process.Start();
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new Exception($"ffmpeg error. ExitCode:{process.ExitCode}");
        }

        return outputPath;
    }

    private async Task UploadTimelapseAsync(
        BlobContainerClient timelapsesBlobContainerClient
        , DateTime targetDate
        , string filePath)
    {
        var blobPath = $"{targetDate:yyyyMMdd}.mp4";
        _logger.LogInformation($"{nameof(UploadTimelapseAsync)} {{FilePath}},{{BlobPath}}", filePath, blobPath);
        using var fileStream = File.OpenRead(filePath);
        var client = timelapsesBlobContainerClient.GetBlobClient(blobPath);
        await client.UploadAsync(fileStream, overwrite: true);
    }
}

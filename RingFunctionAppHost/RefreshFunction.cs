using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RingFunctionAppHost.Logics;
using System.Threading.Tasks;
using static RingFunctionAppHost.SnapshotCollectorFunction;

namespace RingFunctionAppHost
{
    public class RefreshFunction(RingSession ringSession)
    {
        [FunctionName(nameof(RefreshFunction))]
        public async Task<IActionResult> Run(
#pragma warning disable IDE0060
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
#pragma warning restore IDE0060
            [Table("Configuration", Connection = "StorageConnectionString")] TableClient tableClient)
        {
            var config = await tableClient.GetEntityAsync<Config>(nameof(SnapshotCollectorFunction), nameof(Config));
            await ringSession.InitializeAsync(config.Value.OAuthToken);
            config.Value.OAuthToken = ringSession.OAuthTokenString;
            await tableClient.UpdateEntityAsync(config.Value, ETag.All);
            return new OkResult();
        }
    }
}

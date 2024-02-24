using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RingFunctionAppHost.Logics;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static RingFunctionAppHost.SnapshotCollectorFunction;

namespace RingFunctionAppHost
{
    public class SinginFunction(RingSession ringSession)
    {
        [FunctionName(nameof(SinginFunction))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table("Configuration", Connection = "StorageConnectionString")] TableClient tableClient)
        {
            string requestBody;
            using (StreamReader streamReader = new(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var input = JsonConvert.DeserializeObject<Input>(requestBody);

            await ringSession.AuthenticationAsync(input!.UserName, input.Password, input.TwoFactorAuthenticationCode);

            var devices = await ringSession.ListDevicesAsync();

            var config = new Config()
            {
                DoorbotId = devices.Doorbots.First().Id,
                OAuthToken = ringSession.OAuthTokenString,
                LastCollectedDateTime = DateTimeOffset.UtcNow.AddDays(-1),
                PartitionKey = nameof(SnapshotCollectorFunction),
                RowKey = nameof(Config),
                Timestamp = DateTimeOffset.UtcNow,
            };

            await tableClient.UpsertEntityAsync(config, TableUpdateMode.Replace);

            return new OkResult();
        }

        public class Input
        {
            public string UserName { get; set; } = default!;
            public string Password { get; set; } = default!;
            public string TwoFactorAuthenticationCode { get; set; } = default!;
        }
    }
}

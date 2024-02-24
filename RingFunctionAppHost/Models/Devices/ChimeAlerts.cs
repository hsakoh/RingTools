using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class ChimeAlerts
    {
        [JsonPropertyName("connection")]
        public string Connection { get; set; } = default!;
    }
}

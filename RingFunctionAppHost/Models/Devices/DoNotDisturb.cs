using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class DoNotDisturb
    {
        [JsonPropertyName("seconds_left")]
        public int SecondsLeft { get; set; }
    }
}

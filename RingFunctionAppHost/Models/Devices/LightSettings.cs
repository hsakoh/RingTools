using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class LightSettings
    {
        [JsonPropertyName("brightness")]
        public long? Brightness { get; set; }
    }
}

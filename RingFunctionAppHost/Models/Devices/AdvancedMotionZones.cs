using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class AdvancedMotionZones
    {
        [JsonPropertyName("zone1")]
        public Zone Zone1 { get; set; } = default!;

        [JsonPropertyName("zone2")]
        public Zone Zone2 { get; set; } = default!;

        [JsonPropertyName("zone3")]
        public Zone Zone3 { get; set; } = default!;
    }
}

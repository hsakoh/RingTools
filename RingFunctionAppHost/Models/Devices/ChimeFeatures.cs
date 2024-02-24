using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class ChimeFeatures
    {
        [JsonPropertyName("ringtones_enabled")]
        public bool RingtonesEnabled { get; set; }
    }
}

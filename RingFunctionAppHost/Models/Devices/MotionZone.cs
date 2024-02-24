using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class MotionZone
    {
        [JsonPropertyName("enable_audio")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableAudio { get; set; }

        [JsonPropertyName("active_motion_filter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ActiveMotionFilter { get; set; }

        [JsonPropertyName("sensitivity")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Sensitivity { get; set; }

        [JsonPropertyName("advanced_object_settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AdvancedObjectSettings AdvancedObjectSettings { get; set; } = default!;

        [JsonPropertyName("zone1")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Zone Zone1 { get; set; } = default!;

        [JsonPropertyName("zone2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Zone Zone2 { get; set; } = default!;

        [JsonPropertyName("zone3")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Zone Zone3 { get; set; } = default!;

        [JsonPropertyName("pir_settings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PirSettings PirSettings { get; set; } = default!;
    }
}

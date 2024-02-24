using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class AdvancedObjectSettings
    {
        [JsonPropertyName("human_detection_confidence")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HumanDetectionConfidence HumanDetectionConfidence { get; set; } = default!;

        [JsonPropertyName("motion_zone_overlap")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HumanDetectionConfidence MotionZoneOverlap { get; set; } = default!;

        [JsonPropertyName("object_time_overlap")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HumanDetectionConfidence ObjectTimeOverlap { get; set; } = default!;

        [JsonPropertyName("object_size_minimum")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HumanDetectionConfidence ObjectSizeMinimum { get; set; } = default!;

        [JsonPropertyName("object_size_maximum")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HumanDetectionConfidence ObjectSizeMaximum { get; set; } = default!;
    }
}

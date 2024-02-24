﻿using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class PirSettings
    {
        [JsonPropertyName("sensitivity1")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Sensitivity1 { get; set; }

        [JsonPropertyName("sensitivity2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Sensitivity2 { get; set; }

        [JsonPropertyName("sensitivity3")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Sensitivity3 { get; set; }

        [JsonPropertyName("zone_mask")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ZoneMask { get; set; }
    }
}

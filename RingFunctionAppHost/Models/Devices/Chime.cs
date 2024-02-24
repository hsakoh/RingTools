using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class Chime
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; } = default!;

        [JsonPropertyName("time_zone")]
        public string TimeZone { get; set; } = default!;

        [JsonPropertyName("firmware_version")]
        public string FirmwareVersion { get; set; } = default!;

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = default!;

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; } = default!;

        [JsonPropertyName("settings")]
        public ChimeSettings Settings { get; set; } = default!;

        [JsonPropertyName("features")]
        public ChimeFeatures Features { get; set; } = default!;

        [JsonPropertyName("owned")]
        public bool Owned { get; set; }

        [JsonPropertyName("alerts")]
        public ChimeAlerts Alerts { get; set; } = default!;

        [JsonPropertyName("do_not_disturb")]
        public DoNotDisturb DoNotDisturb { get; set; } = default!;

        [JsonPropertyName("stolen")]
        public bool Stolen { get; set; }

        [JsonPropertyName("owner")]
        public Owner Owner { get; set; } = default!;
    }
}

using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class Doorbot
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; } = default!;

        [JsonPropertyName("time_zone")]
        public string TimeZone { get; set; } = default!;

        [JsonPropertyName("subscribed")]
        public bool? Subscribed { get; set; }

        [JsonPropertyName("subscribed_motions")]
        public bool? SubscribedMotions { get; set; }

        [JsonPropertyName("battery_life")]
        public string? BatteryLife { get; set; }

        [JsonPropertyName("external_connection")]
        public bool? ExternalConnection { get; set; }

        [JsonPropertyName("firmware_version")]
        public string FirmwareVersion { get; set; } = default!;

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = default!;

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; } = default!;

        [JsonPropertyName("features")]
        public DoorbotFeatures Features { get; set; } = default!;

        [JsonPropertyName("owned")]
        public bool? Owned { get; set; }

        [JsonPropertyName("alerts")]
        public DoorbotAlerts Alerts { get; set; } = default!;

        [JsonPropertyName("motion_snooze")]
        public object MotionSnooze { get; set; } = default!;

        [JsonPropertyName("stolen")]
        public bool? Stolen { get; set; }

        [JsonPropertyName("owner")]
        public Owner Owner { get; set; } = default!;
    }
}

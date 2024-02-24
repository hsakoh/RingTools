using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class ChimeSettings
    {
        [JsonPropertyName("volume")]
        public int Volume { get; set; }

        [JsonPropertyName("ding_audio_user_id")]
        public string DingAudioUserId { get; set; } = default!;

        [JsonPropertyName("ding_audio_id")]
        public string DingAudioId { get; set; } = default!;

        [JsonPropertyName("motion_audio_user_id")]
        public string MotionAudioUserId { get; set; } = default!;

        [JsonPropertyName("motion_audio_id")]
        public string MotionAudioId { get; set; } = default!;
    }
}

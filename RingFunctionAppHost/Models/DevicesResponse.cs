using RingFunctionAppHost.Models.Devices;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models
{
    public class DevicesResponse
    {
        /// <summary>
        /// All Ring doorbots
        /// </summary>
        [JsonPropertyName("doorbots")]
        public List<Doorbot> Doorbots { get; set; } = default!;

        /// <summary>
        /// All Authorized Ring doorbots
        /// </summary>
        [JsonPropertyName("authorized_doorbots")]
        public List<Doorbot> AuthorizedDoorbots { get; set; } = default!;

        /// <summary>
        /// All Ring chimes
        /// </summary>
        [JsonPropertyName("chimes")]
        public List<Chime> Chimes { get; set; } = default!;

        /// <summary>
        /// All Ring stickup cameras
        /// </summary>
        [JsonPropertyName("stickup_cams")]
        public List<StickupCam> StickupCams { get; set; } = default!;
    }
}

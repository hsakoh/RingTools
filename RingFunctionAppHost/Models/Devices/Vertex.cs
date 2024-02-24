using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class Vertex
    {
        [JsonPropertyName("x")]
        public long? X { get; set; }

        [JsonPropertyName("y")]
        public long? Y { get; set; }
    }
}

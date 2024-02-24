using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class Zone
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("state")]
        public long? State { get; set; } = default!;

        [JsonPropertyName("vertex1")]
        public Vertex Vertex1 { get; set; } = default!;

        [JsonPropertyName("vertex2")]
        public Vertex Vertex2 { get; set; } = default!;

        [JsonPropertyName("vertex3")]
        public Vertex Vertex3 { get; set; } = default!;

        [JsonPropertyName("vertex4")]
        public Vertex Vertex4 { get; set; } = default!;

        [JsonPropertyName("vertex5")]
        public Vertex Vertex5 { get; set; } = default!;

        [JsonPropertyName("vertex6")]
        public Vertex Vertex6 { get; set; } = default!;

        [JsonPropertyName("vertex7")]
        public Vertex Vertex7 { get; set; } = default!;

        [JsonPropertyName("vertex8")]
        public Vertex Vertex8 { get; set; } = default!;
    }
}

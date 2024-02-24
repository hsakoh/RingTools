using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models.Devices
{
    public class Owner
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = default!;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = default!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = default!;
    }
}

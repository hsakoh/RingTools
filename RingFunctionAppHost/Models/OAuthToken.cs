using System;
using System.Text.Json.Serialization;

namespace RingFunctionAppHost.Models;

#pragma warning disable CS8618

public class OAuthToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    private int _expiresInSeconds;

    [JsonPropertyName("expires_in")]
    public int ExpiresInSeconds
    {
        get { return _expiresInSeconds; }
        set { _expiresInSeconds = value; ExpiresAt = DateTime.Now.AddSeconds(value); }
    }

    public DateTime ExpiresAt { get; private set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("created_at")]
    public int CreatedAtTicks { get; set; }

    public DateTime CreatedAt
    {
        get { return new DateTime(1970, 1, 1).AddSeconds(CreatedAtTicks); }
    }
}

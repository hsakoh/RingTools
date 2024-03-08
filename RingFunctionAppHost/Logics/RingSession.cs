using Microsoft.Extensions.Logging;
using RingFunctionAppHost.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace RingFunctionAppHost.Logics;

public class RingSession(HttpClient httpClient, ILogger<RingSession> logger)
{
    private OAuthToken OAuthToken { get; set; } = default!;

    public async Task AuthenticationAsync(string userName, string password, string twoFactorAuthenticationCode)
    {
        logger.LogInformation($"{nameof(AuthenticationAsync)} {{userName}}", userName);
        try
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://oauth.ring.com/oauth/token"),
                Content = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
                        { "grant_type", "password" },
                        { "username", userName },
                        { "password", password },
                        { "client_id", "ring_official_android" },
                        { "scope", "client" }
                    }
                )
            };
            request.Headers.Add("2fa-support", "true");
            request.Headers.Add("2fa-code", twoFactorAuthenticationCode);
            request.Headers.Add("hardware_id", "f1f3998a-aa33-4ea9-86eb-d5d12e0ec26e");
            request.Headers.TryAddWithoutValidation("User-Agent", "android:com.ringapp");
            using var response = await httpClient.SendAsync(request);
            var responseText = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            logger.LogInformation($"{nameof(AuthenticationAsync)} {{ResponseText}}", responseText);
            OAuthToken = JsonSerializer.Deserialize<OAuthToken>(responseText)!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(AuthenticationAsync)} {{userName}}", userName);
            throw;
        }
    }

    public async Task<DevicesResponse> ListDevicesAsync()
    {
        logger.LogInformation($"{nameof(ListDevicesAsync)}");
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://api.ring.com/clients_api/ring_devices"));
            request.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {OAuthToken.AccessToken}");
            using var response = await httpClient.SendAsync(request);
            var responseText = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            logger.LogInformation($"{nameof(ListDevicesAsync)} {{ResponseText}}", responseText);
            return JsonSerializer.Deserialize<DevicesResponse>(responseText)!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(ListDevicesAsync)}");
            throw;
        }
    }


    public async Task InitializeAsync(string oauthToken)
    {
        OAuthToken = JsonSerializer.Deserialize<OAuthToken>(oauthToken)!;
        await EnsureSessionValidAsync();
    }

    public async Task RefreshSessionAsync(string refreshToken)
    {
        logger.LogInformation($"{nameof(RefreshSessionAsync)} {{RefreshToken}}", refreshToken);
        try
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://oauth.ring.com/oauth/token"),
                Content = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
                        { "grant_type", "refresh_token" },
                        { "refresh_token", refreshToken }
                    }
                )
            };
            request.Headers.Add("2fa-support", "true");
            request.Headers.Add("2fa-code", "");
            request.Headers.Add("hardware_id", "f1f3998a-aa33-4ea9-86eb-d5d12e0ec26e");
            request.Headers.TryAddWithoutValidation("User-Agent", "android:com.ringapp");
            using var response = await httpClient.SendAsync(request);
            var responseText = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            logger.LogInformation($"{nameof(RefreshSessionAsync)} {{ResponseText}}", responseText);
            OAuthToken = JsonSerializer.Deserialize<OAuthToken>(responseText)!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(RefreshSessionAsync)} {{RefreshToken}}", refreshToken);
            throw;
        }
    }

    public async Task EnsureSessionValidAsync()
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(OAuthToken.AccessToken);
        if (jwtSecurityToken.ValidTo < DateTime.UtcNow)
        {
            await RefreshSessionAsync(OAuthToken.RefreshToken);
        }
    }


    public async Task<OrchestratorTimelineResponse> GetOrchestratorTimelineAsync(int doorbotId, DateTimeOffset startTime, DateTimeOffset endTime, bool? isOderByAsc = null, int? limit = null, string? paginationKey = null)
    {
        logger.LogInformation($"{nameof(GetOrchestratorTimelineAsync)} {{DoorbotId}},{{StartTime}},{{EndTime}}", doorbotId, startTime, endTime);

        try
        {
            await EnsureSessionValidAsync();

            var query = HttpUtility.ParseQueryString("");
            query.Add("start_time", startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
            query.Add("end_time", endTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
            if (isOderByAsc is not null)
            {
                query.Add("order", isOderByAsc.Value ? "asc" : "desc");
            }
            if (limit is not null)
            {
                query.Add("limit", limit.ToString());
            }
            if (paginationKey is not null)
            {
                query.Add("pagination_key", paginationKey);
            }
            var uriBuilder = new UriBuilder(new Uri($"https://api.ring.com/evm/v2/timeline/devices/{doorbotId}"))
            {
                Query = query.ToString()
            };

            using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
            request.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {OAuthToken.AccessToken}");
            using var response = await httpClient.SendAsync(request);
            var responseText = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            logger.LogInformation($"{nameof(GetOrchestratorTimelineAsync)} {{ResponseText}}", responseText);
            var orchestratorTimeline = JsonSerializer.Deserialize<OrchestratorTimelineResponse>(responseText);
            return orchestratorTimeline!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(GetOrchestratorTimelineAsync)}");
            throw;
        }
    }

    public string OAuthTokenString { get => JsonSerializer.Serialize(OAuthToken); }
}

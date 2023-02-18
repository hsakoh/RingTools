using Microsoft.Extensions.Logging;
using RingFunctionAppHost.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace RingFunctionAppHost.Logics;

public class RingSession
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RingSession> _logger;

    private OAuthToken OAuthToken { get; set; } = default!;

    public RingSession(HttpClient httpClient, ILogger<RingSession> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    public async Task InitializeAsync(string refreshToken)
    {
        await RefreshSessionAsync(refreshToken);
    }

    public async Task RefreshSessionAsync(string refreshToken)
    {
        _logger.LogInformation($"{nameof(RefreshSessionAsync)} {{RefreshToken}}", refreshToken);
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
            using var response = await _httpClient.SendAsync(request);
            var responseText = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            _logger.LogInformation($"{nameof(RefreshSessionAsync)} {{ResponseText}}", responseText);
            OAuthToken = JsonSerializer.Deserialize<OAuthToken>(responseText)!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(RefreshSessionAsync)} {{RefreshToken}}", refreshToken);
            throw;
        }
    }
    public async Task EnsureSessionValidAsync()
    {
        if (OAuthToken.ExpiresAt < DateTime.UtcNow)
        {
            await RefreshSessionAsync(OAuthToken.RefreshToken);
        }
    }


    public async Task<OrchestratorTimelineResponse> GetOrchestratorTimelineAsync(int doorbotId, DateTimeOffset startTime, DateTimeOffset endTime, bool? isOderByAsc = null, int? limit = null, string? paginationKey = null)
    {
        _logger.LogInformation($"{nameof(GetOrchestratorTimelineAsync)} {{DoorbotId}},{{StartTime}},{{EndTime}}", doorbotId, startTime, endTime);

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
            using var response = await _httpClient.SendAsync(request);
            var responseText = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            _logger.LogInformation($"{nameof(GetOrchestratorTimelineAsync)} {{ResponseText}}", responseText);
            var orchestratorTimeline = JsonSerializer.Deserialize<OrchestratorTimelineResponse>(responseText);
            return orchestratorTimeline!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(GetOrchestratorTimelineAsync)}");
            throw;
        }
    }

    public string CurrentRefreshToken { get => OAuthToken.RefreshToken; }
}

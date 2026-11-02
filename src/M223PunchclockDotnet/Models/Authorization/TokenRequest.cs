using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Models.Authorization;

public record TokenRequest
{
    [JsonPropertyName("grant_type")] public string? GrantType { get; init; }

    [JsonPropertyName("code")] public string? Code { get; init; }

    [JsonPropertyName("redirect_uri")] public string? RedirectUri { get; init; }

    [JsonPropertyName("client_id")] public string? ClientId { get; init; }

    [JsonPropertyName("client_secret")] public string? ClientSecret { get; init; }
}
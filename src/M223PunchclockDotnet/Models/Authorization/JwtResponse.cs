using System.Text.Json.Serialization;

namespace M223PunchclockDotnet.Models.Authorization;

public record JwtResponse
{
    [JsonPropertyName("access_token")] public required string AccessToken { get; init; }

    [JsonPropertyName("token_type")] public string TokenType { get; init; } = "Bearer";

    [JsonPropertyName("expires_in")] public int ExpiresIn { get; init; } = 3600;

    [JsonPropertyName("scope")] public required string Scope { get; init; }
}
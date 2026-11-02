namespace M223PunchclockDotnet.Models.Authorization;

public record OAuthCode
{
    public bool Expired => DateTime.UtcNow > ExpiresAt;
    public required string ClientId { get; init; }
    public required string RedirectUri { get; init; }
    public DateTime ExpiresAt { get; init; }
}
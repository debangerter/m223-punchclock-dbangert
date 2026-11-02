namespace M223PunchclockDotnet.Models.Authorization;

public record OAuthClient
{
    public required string ClientId { get; init; }
    public ICollection<string> RedirectUris { get; init; } = [];
}
using M223PunchclockDotnet.Models.Authorization;

namespace M223PunchclockDotnet.Services;

public interface IAuthService
{
    Task<OAuthClient?> FindClientAsync(string clientId);
    JwtResponse GenerateToken(int id);

    Task<string> IssueCodeAsync(string clientId, string redirectUri, Guid userId, string codeChallenge,
        TimeSpan lifeSpan);

    Task<OAuthCode?> TakeCodeAsync(string codeKey);
    Task<bool> CheckSecretAsync(string clientId, string clientSecret);
}
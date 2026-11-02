using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using JetBrains.Annotations;
using M223PunchclockDotnet.Models.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace M223PunchclockDotnet.Services;

[UsedImplicitly]
public class AuthService : IAuthService
{
    private readonly RSA _privateKey;

    public AuthService()
    {
        _privateKey = RSA.Create();
        _privateKey.ImportFromPem(File.ReadAllText("Keys/private.key"));
    }

    public Task<OAuthClient?> FindClientAsync(string clientId)
    {
        return Task.FromResult<OAuthClient?>(new OAuthClient
        {
            ClientId = clientId,
            RedirectUris = ["http://localhost:5121/swagger/oauth2-redirect.html"]
        });
    }

    public JwtResponse GenerateToken(int id)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        RsaSecurityKey key = new(_privateKey);

        ICollection<Claim> claims =
        [
            new(ClaimTypes.Name, "TestUser"),
            new(ClaimTypes.UserData, id.ToString()),
            new(ClaimTypes.Role, "Admin")
        ];

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return new JwtResponse
        {
            AccessToken = tokenHandler.WriteToken(token),
            Scope = "api"
        };
    }

    public Task<string> IssueCodeAsync(string clientId, string redirectUri, Guid userId, string codeChallenge,
        TimeSpan lifeSpan)
    {
        // Redis entry Key: unique_key, Value: data, TTL: lifeSpan 

        return Task.FromResult("code_data");
    }

    public Task<OAuthCode?> TakeCodeAsync(string codeKey)
    {
        // Redis get Key: codeKey

        // JSON parse data -> OAuthCode

        return Task.FromResult<OAuthCode?>(new OAuthCode
        {
            ClientId = "TestClientId",
            RedirectUri = "http://localhost:5121/swagger/oauth2-redirect.html",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
    }

    public Task<bool> CheckSecretAsync(string clientId, string clientSecret)
    {
        // Redis check Key: clientId, Value: clientSecret

        return Task.FromResult(true);
    }
}
using System.Net.Mime;
using System.Security.Claims;
using M223PunchclockDotnet.Models.Authorization;
using M223PunchclockDotnet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace M223PunchclockDotnet.Controllers;

[ApiController]
[Route("login/oauth/[action]")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest, Description = "The request is invalid")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    public async Task<IActionResult> AuthorizeAsync(
        [FromQuery(Name = "client_id")] string? clientId,
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "response_type")] string responseType,
        [FromQuery(Name = "state")] string state,
        [FromQuery(Name = "scope")] string scope,
        [FromQuery(Name = "code_challenge")] string? codeChallenge
    )
    {
        clientId ??= "TestClientId";
        codeChallenge ??= "code_challenge";

        OAuthClient? client = await service.FindClientAsync(clientId);
        if (client is null || responseType != "code" || client.RedirectUris.Contains(redirectUri) is false)
            return BadRequest("Invalid request"); // Or redirect with error

        if (User.Identity?.IsAuthenticated is not true)
            return Challenge(); // Or redirect to login page

        string code = await service.IssueCodeAsync(clientId, redirectUri, ClaimsPrincipal.GetUserId(), codeChallenge,
            TimeSpan.FromMinutes(1));

        string url = QueryHelpers.AddQueryString(redirectUri, new Dictionary<string, string?>
        {
            ["code"] = code,
            ["state"] = state
        });

        return Redirect(url);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
    public async Task<IActionResult> Access_TokenAsync(
        [FromForm(Name = "grant_type")] string grantType,
        [FromForm(Name = "code")] string codeKey,
        [FromForm(Name = "redirect_uri")] string redirectUri,
        [FromForm(Name = "client_id")] string clientId,
        [FromForm(Name = "client_secret")] string clientSecret
    )
    {
        if (grantType != "authorization_code")
            return Unauthorized("Unsupported grant type");

        OAuthCode? code = await service.TakeCodeAsync(codeKey);

        if (code is null || code.Expired || clientId != code.ClientId || redirectUri != code.RedirectUri)
            return Unauthorized("Invalid grant");

        if (await service.CheckSecretAsync(clientId, clientSecret) is false)
            return Unauthorized("Invalid client");

        return Ok(service.GenerateToken(42));
    }
}
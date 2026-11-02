using System.Security.Claims;

namespace M223PunchclockDotnet.Models.Authorization;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal principal)
    {
        public static Guid GetUserId()
        {
            return Guid.Parse("DB5D7286-BA20-4685-88B8-A7A51E42580D");
        }
    }
}
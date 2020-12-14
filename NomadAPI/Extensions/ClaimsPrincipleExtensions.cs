using System.Security.Claims;

namespace NomadAPI.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;

        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        }
    }
}

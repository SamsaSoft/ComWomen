using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Admin.Common
{
    public abstract class BasePageModel : PageModel
    {
        public string GetCurrentUserId()
        {
            return GetUserId(User);
        }

        public static string GetUserId(ClaimsPrincipal user)
        {
            if (user == null || !user.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                return string.Empty;
            return user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public override SignOutResult SignOut(params string[] authenticationSchemes)
        {
            return base.SignOut(authenticationSchemes);
        }

        public override SignOutResult SignOut(AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return base.SignOut(properties, authenticationSchemes);
        }
    }
}

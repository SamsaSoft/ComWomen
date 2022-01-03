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
            if(User == null || !User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                return string.Empty;
            return User.Claims.First(x=>x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}

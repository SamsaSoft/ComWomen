using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    public abstract class BaseController : Controller
    {
        public string GetUserId()
        {
            if (User == null || !User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                return string.Empty;
            return User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}

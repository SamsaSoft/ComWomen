using Admin.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Admin.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        bool IsPostLanguage(HttpContext httpContext) =>
            httpContext.Request.Method == "POST"
            && httpContext.Request.Query["handler"].Any(x => x == "language");

        static readonly IList<string> CultureNames = Settings.ActiveLanguages
                    .Select(c => c.ToString())
                    .ToList();

        async Task InitLanguageClaim(HttpContext httpContext,
                                     SignInManager<ApplicationUser> signInManager,
                                     ApplicationUser user)
        {
            var lang = httpContext.Session.GetString("language");
            if (string.IsNullOrEmpty(lang))
            {
                var requestCulture = httpContext.Features.Get<IRequestCultureFeature>();
                if (requestCulture != null)
                {
                    lang = httpContext.User.Language();
                    if (string.IsNullOrEmpty(lang) )
                    {
                        lang = requestCulture.RequestCulture.UICulture.Name;
                        if (user == null)//annonimus
                        {
                            ((ClaimsIdentity)httpContext.User.Identity).AddClaim(new Claim("language", lang));
                        }
                        else
                        {
                           await UpdateLanguage(httpContext, signInManager, user, lang);
                        }
                    }
                    else if(requestCulture.RequestCulture.UICulture.Name != lang) 
                    {
                        await UpdateLanguage(httpContext, signInManager, user, lang);
                        httpContext.Response.Redirect(httpContext.Request.Path);
                    }
                }
                httpContext.Session.SetString("language", lang);
            }
        }

        async Task UpdateLanguage(HttpContext httpContext, SignInManager<ApplicationUser> signInManager, ApplicationUser user, string lang)
        {
            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            if (user != null)
            {
                var userManager = signInManager.UserManager;
                var claim = httpContext.User.Claims.FirstOrDefault((x) => x.Type == "language");
                if (claim == null)
                {
                    await userManager.AddClaimAsync(user, new Claim("language", lang));
                }
                else
                {
                    await userManager.ReplaceClaimAsync(user, claim, new Claim("language", lang));
                }
                await userManager.UpdateAsync(user);
                await signInManager.RefreshSignInAsync(user);
            }
            httpContext.Session.SetString("language", lang);
        }

        public async Task InvokeAsync(HttpContext httpContext, SignInManager<ApplicationUser> signInManager)
        {
            if (httpContext.Request.Path.Value.ToLower() == "/identity/account/login")
            {
                httpContext.Session.Remove("language");
                await _next.Invoke(httpContext);
                return;
            }
            var userManager = signInManager.UserManager;
            var userId = BasePageModel.GetUserId(httpContext.User);
            var user = !string.IsNullOrEmpty(userId) ? await userManager.FindByIdAsync(userId) : null;
            await InitLanguageClaim(httpContext, signInManager, user);
            if (IsPostLanguage(httpContext))
            {
                var requestCulture = httpContext.Features.Get<IRequestCultureFeature>();
                var itemIndex = CultureNames.IndexOf(requestCulture.RequestCulture.Culture.Name);
                if (++itemIndex > CultureNames.Count - 1)
                    itemIndex = 0;
                var returnUrl = httpContext.Request.Query["returnUrl"];
                await UpdateLanguage(httpContext, signInManager, user, CultureNames[itemIndex]);
                httpContext.Response.Redirect(returnUrl);
            }
            await _next.Invoke(httpContext);
        }
    }

    public static class LanguageMiddlewareExtensions
    {
        public static IApplicationBuilder UseLanguageMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LanguageMiddleware>();
        }
    }
}

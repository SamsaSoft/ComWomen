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

        Task InitLanguageClaim(HttpContext httpContext, ApplicationUser user)
        {
            if (user == null)
            {
                var requestCulture = httpContext.Features.Get<IRequestCultureFeature>();
                ((ClaimsIdentity)httpContext.User.Identity)
                    .AddClaim(new Claim("language", requestCulture.RequestCulture.Culture.Name));
            }
            return Task.CompletedTask;
        }

        async Task UpdateLanguage(HttpContext httpContext, SignInManager<ApplicationUser> signInManager, ApplicationUser user, string lang)
        {
            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            httpContext.Session.SetString("language", lang);
            if (user == null)
            {
                return;
            }
            var claim = httpContext.User.Claims.FirstOrDefault((x) => x.Type == "language");
            if (claim?.Value == lang)
                return;
            var userManager = signInManager.UserManager;
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

        async Task OnPostLanguage(HttpContext httpContext, SignInManager<ApplicationUser> signInManager, ApplicationUser user)
        {
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
        }

        public async Task InvokeAsync(HttpContext httpContext, SignInManager<ApplicationUser> signInManager)
        {
            var userManager = signInManager.UserManager;
            var user = await userManager.GetUserAsync(httpContext.User);
            await InitLanguageClaim(httpContext, user);
            await OnPostLanguage(httpContext, signInManager, user);
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
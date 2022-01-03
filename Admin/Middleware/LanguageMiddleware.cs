using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
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

        public Task Invoke(HttpContext httpContext)
        {
            if (IsPostLanguage(httpContext))
            {
                var culture = httpContext.Request.Form["culture"];
                var returnUrl = httpContext.Request.Query["returnUrl"];
                httpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
                httpContext.Response.Redirect(returnUrl);
                return Task.CompletedTask;
            }
            return _next(httpContext);
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

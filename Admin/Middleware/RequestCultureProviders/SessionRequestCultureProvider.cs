using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace Admin.Middleware.RequestCultureProviders
{
    public class SessionRequestCultureProvider : RequestCultureProvider
    {
        public static string CultureKey { get; } = "language";

        public static string UICultureKey { get; } = "ui-language";

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext.Session == null)
            {
                return Task.FromResult<ProviderCultureResult>(null);
            }
            var language = httpContext.Session.GetString(CultureKey);
            return !string.IsNullOrEmpty(language)
                ? Task.FromResult(new ProviderCultureResult(language, language))
                : Task.FromResult<ProviderCultureResult>(null);
        }
    }
}

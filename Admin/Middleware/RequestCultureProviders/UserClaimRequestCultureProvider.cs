using Microsoft.AspNetCore.Localization;

namespace Admin.Middleware.RequestCultureProviders
{
    public class UserClaimRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return Task.FromResult<ProviderCultureResult>(null);
            }
            var language = httpContext.User.GetLanguage();
            return !string.IsNullOrEmpty(language)
                ? Task.FromResult(new ProviderCultureResult(language, language))
                : Task.FromResult<ProviderCultureResult>(null);
        }
    }
}

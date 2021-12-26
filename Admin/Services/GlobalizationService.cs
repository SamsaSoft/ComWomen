using Core.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Services
{
    public class GlobalizationService : IGlobalizationService
    {
        private readonly IViewLocalizer localizer;
        private readonly IRequestCultureFeature requestCulture;

        public GlobalizationService(IViewLocalizer localizer, IRequestCultureFeature requestCulture)
        {
            this.localizer = localizer;
            this.requestCulture = requestCulture;
        }
        public string this[int key] => throw new NotImplementedException();

        public string this[string key] => localizer[key].IsResourceNotFound ? key : localizer[key].Value;

        public IEnumerable<CultureInfo> SupportedCultures => Enum.GetNames<LanguageEnum>().Select(x => new CultureInfo(x)).ToList();

        public CultureInfo ActiveUiCulture { get => requestCulture?.RequestCulture.UICulture; }
    }

    public static class GlobalizationServiceExtensions 
    {
        public static IServiceCollection RegisterGlobalizationService(this IServiceCollection services) 
        {
            services.AddLocalization(options => options.ResourcesPath = "Language");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = Enum.GetNames<LanguageEnum>().Select(x => new CultureInfo(x)).ToList();
                options.DefaultRequestCulture = new RequestCulture(LanguageEnum.ru.ToString());
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            //services.AddScoped<IGlobalizationService, GlobalizationService>();
            return services;
        }        
    }
}

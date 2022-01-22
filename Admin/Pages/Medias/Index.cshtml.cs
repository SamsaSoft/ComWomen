using Core.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Pages.Medias
{
    public class IndexModel : BaseMediaPageModel
    {
        public Language ActiveLanguage { get; set; }

        public IndexModel(IMediaService mediaService):base(mediaService)
        {
        }

        [BindProperty]
        public IEnumerable<Media> Medias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            ActiveLanguage = Enum.Parse<Language>(requestCulture.RequestCulture.Culture.Name);
            Medias = await _mediaService.GetAll();
            return Page();
        }
    }
}

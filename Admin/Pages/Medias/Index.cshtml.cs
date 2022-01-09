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
            ActiveLanguage = Enum.Parse<Language>(HttpContext.User.GetLanguage());
            Medias = await _mediaService.GetAll();
            return Page();
        }
    }
}

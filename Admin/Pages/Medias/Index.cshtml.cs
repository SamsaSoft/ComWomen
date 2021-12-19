using Admin.Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Medias
{
    public class IndexModel : BaseMediaPageModel
    {
        public LanguageEnum ActiveLanguage { get; set; }

        public IndexModel(IMediaService mediaService):base(mediaService)
        {
        }

        [BindProperty]
        public IEnumerable<Media> Medias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ActiveLanguage = LanguageEnum.ky;
            Medias = await _mediaService.GetAll();
            return Page();
        }
    }
}

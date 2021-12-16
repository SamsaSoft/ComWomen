using Admin.Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Medias
{
    public class IndexModel : BasePageModel
    {
        private readonly IMediaService _mediaService;

        public LanguageEnum ActiveLanguage { get; set; }

        public IndexModel(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [BindProperty]
        public IEnumerable<Media> Medias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ActiveLanguage = LanguageEnum.ky;
            Medias = await _mediaService.GetAllWithType(Core.Enums.MediaTypeEnum.Photo);
            return Page();
        }
    }
}

using Admin.Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Medias
{
    public class DetailsModel : BasePageModel
    {
        public LanguageEnum ActiveLanguage { get; set; }

        private readonly IMediaService _mediaService;

        public Media Media { get; set; }

        public DetailsModel(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [BindProperty(SupportsGet = true)]
        public int? MediaId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!MediaId.HasValue)
            {
                return NotFound();
            }
            Media = await _mediaService.GetById(MediaId.Value);
            return Page();
        }
    }
}

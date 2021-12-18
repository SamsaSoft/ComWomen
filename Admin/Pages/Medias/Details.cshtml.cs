using Admin.Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Medias
{
    public class DetailsModel : BaseMediaPageModel
    {
        public LanguageEnum ActiveLanguage { get; set; }

        public Media Media { get; set; }

        public IEnumerable<LanguageEnum> ActiveLanguages => Media.MediaTranslations.Select(x => x.LanguageId);

        public DetailsModel(IMediaService mediaService):base(mediaService)
        {

        }

        [BindProperty(SupportsGet = true)]
        public int MediaId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Media = await _mediaService.GetById(MediaId);
                return Page();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

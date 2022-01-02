using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Pages.Medias
{
    public class DeleteModel : BaseMediaPageModel
    {
        public LanguageEnum ActiveLanguage { get; set; }
        public DeleteModel(IMediaService mediaService):base(mediaService)
        {
        }
        [BindProperty]
        public Media Media { get; set; }

        [BindProperty(SupportsGet = true)]
        public int MediaId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ActiveLanguage = Settings.DefaultLanguage;
                Media = await _mediaService.GetById(MediaId);
                return Page();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //TODO check and remove files
            await _mediaService.DelteById(MediaId);
            return RedirectToPage("index");
        }
    }
}

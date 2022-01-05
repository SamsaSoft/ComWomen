using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Pages.Medias
{
    public class CreateModel : BaseMediaPageModel
    {
        public CreateModel(IMediaService mediaService) : base(mediaService)
        {
        }

        [BindProperty]
        public Media Media { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Media = new Media
            {
                Translations = new List<MediaTranslation>(Settings.ActiveLanguages.Select(x => new MediaTranslation { LanguageId = x }))
            };

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Media.AuthorId = GetCurrentUserId();
            Media.CreatedAt = DateTime.UtcNow;
            var result = await _mediaService.Create(Media);
            if (!result.IsSuccess)
            {
                ModelState.TryAddModelError("MediaTranslation_url", result.Message);
                return Page();
            }
            return RedirectToPage("index");
        }
    }
}

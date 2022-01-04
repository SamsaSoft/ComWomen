using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Pages.Medias
{
    public class CreateModel : BaseMediaPageModel
    {
        private readonly IFileService _fileService;

        public CreateModel(IMediaService mediaService, IFileService fileService) : base(mediaService)
        {
            _fileService = fileService;
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
            var result = await _fileService.CreateFile(Media);
            if (!result.IsSuccess)
            {
                ModelState.TryAddModelError("MediaTranslation_url", result.Message);
                return Page();
            }
            Media.AuthorId = GetCurrentUserId();
            await _mediaService.Create(Media);
            return RedirectToPage("index");
        }
    }
}

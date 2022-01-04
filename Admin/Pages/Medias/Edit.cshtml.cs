using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Admin.Pages.Medias
{
    public class EditModel : BaseMediaPageModel
    {
        private readonly IFileService _fileService;

        public EditModel(IMediaService mediaService, IFileService fileService) : base(mediaService)
        {
            _fileService = fileService;
        }
        [BindProperty]
        public Media Media { get; set; }

        public IDictionary<string, string> MediaDirectoryDictionary => Enum
            .GetValues<MediaType>()
            .ToDictionary(x => MediaTypeIdToClassName(x), x => x.ToString());

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _fileService.UpdateFile(Media);
            if (!result.IsSuccess)
            {
                ModelState.TryAddModelError("MediaTranslation_url", result.Message);
                return Page();
            }
            Media.EditorId = GetCurrentUserId();
            Media.EditedAt = DateTime.UtcNow;
            await _mediaService.Update(Media);
            return RedirectToPage("index");
        }
    }
}

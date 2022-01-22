using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Admin.Pages.Medias
{
    public class EditModel : BaseMediaPageModel
    {
        public EditModel(IMediaService mediaService) : base(mediaService)
        {
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
            Media.EditorId = GetCurrentUserId();
            Media.EditedAt = DateTime.UtcNow;
            var result = await _mediaService.Update(Media);
            if (!result.IsSuccess)
            {
                ModelState.TryAddModelError("MediaTranslation_url", result.Message);
                return Page();
            }
            return RedirectToPage("index");
        }
    }
}

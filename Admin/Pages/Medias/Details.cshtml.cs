using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Pages.Medias
{
    public class DetailsModel : BaseMediaPageModel
    {
        public Media Media { get; set; }


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

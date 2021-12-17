using Admin.Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Medias
{
    public class DeleteModel : BasePageModel
    {

        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _webHost;

        public DeleteModel(IMediaService mediaService, IWebHostEnvironment webHost)
        {
            _mediaService = mediaService;
            _webHost = webHost;
        }
        [BindProperty]
        public Media Media { get; set; }

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
            //TODO check and remove files
            await _mediaService.DelteById(MediaId);
            return RedirectToPage("index");
        }
    }
}

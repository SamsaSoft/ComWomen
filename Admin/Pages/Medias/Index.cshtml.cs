using Admin.Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Medias
{
    public class IndexModel : BasePageModel
    {
        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _webHost;

        public IndexModel(IMediaService mediaService, IWebHostEnvironment webHost)
        {
            _mediaService = mediaService;
            _webHost = webHost;
        }

        [BindProperty]
        public IEnumerable<Media> Medias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Medias = await _mediaService.GetAllWithType(Core.Enums.MediaTypeEnum.Photo);
            return Page();
        }
    }
}

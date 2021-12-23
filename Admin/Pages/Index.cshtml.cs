using Admin.Common;
using Core.DataAccess.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediaService _mediaService;

        public IndexModel(ILogger<IndexModel> logger, IMediaService mediaService)
        {
            _logger = logger;
            _mediaService = mediaService;
        }

        public async Task OnGet()
        {
            try
            {
                //var userId = GetCurrentUserId();
                //var item = new Media
                //{
                //    AuthorId = "bf7337fc-6490-4df4-a877-2d5bb8e6a6f9",
                //    Title = "bla",
                //    Url = "url",
                //    MediaTypeId = MediaTypeEnum.Audio,
                //    CreatedAt = DateTime.UtcNow
                //};

                //await _mediaService.Upload(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
           
        }
    }
}
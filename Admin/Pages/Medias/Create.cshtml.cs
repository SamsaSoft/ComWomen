using Admin.Common;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Admin.Pages.Medias
{
    public class CreateModel : BasePageModel
    {
        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _webHost;

        public CreateModel(IMediaService mediaService, IWebHostEnvironment webHost)
        {
            _mediaService = mediaService;
            _webHost = webHost;
        }
        [BindProperty]
        public Media Media { get; set; }
        [BindProperty]
        public Dictionary<LanguageEnum, IFormFile> Files { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Media = new Media
            {
                MediaTypeId = MediaTypeEnum.Photo,
                MediaTranslations = CreateMediaTranslationsAllLanguages().ToList(),
            };
            Files = CreateFileDictionaryAllLanguages();
            await Task.CompletedTask;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Media.AuthorId = GetCurrentUserId();
            Media.CreatedAt = DateTime.UtcNow;
            await ProcessingAttachFiles();
            await _mediaService.Upload(Media);
            return RedirectToPage("index");
        }

        private async Task ProcessingAttachFiles()
        {
            var wwwPath = _webHost.WebRootPath;
            var mediaPath = Path.Combine(wwwPath, "images");
            foreach (var item in Enum.GetValues<LanguageEnum>())
            {
                if (Files[item] != null)
                {
                    var filePath = Path.Combine(mediaPath, Files[item].FileName);
                    Media[item].Url = Files[item].FileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        continue;
                    }
                    using var stream = new FileStream(filePath, FileMode.CreateNew);
                    await Files[item].CopyToAsync(stream);
                }
            }
        }

        private static Dictionary<LanguageEnum, IFormFile> CreateFileDictionaryAllLanguages()
        {
            Dictionary<LanguageEnum, IFormFile> files = new Dictionary<LanguageEnum, IFormFile>();
            foreach (var item in Enum.GetValues<LanguageEnum>())
            {
                files.Add(item, null);
            }
            return files;
        }
        private static IEnumerable<MediaTranslation> CreateMediaTranslationsAllLanguages()
        {
            foreach (var item in Enum.GetValues<LanguageEnum>())
            {
                yield return new MediaTranslation() { LanguageId = item };
            }
        }
    }
}

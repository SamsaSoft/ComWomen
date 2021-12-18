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
        public IEnumerable<LanguageEnum> ActiveLanguages => Enum.GetValues<LanguageEnum>().Where(x=>x!= LanguageEnum.en);
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
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
            CreateDirectoryIfNotExists(mediaPath);
            foreach (var item in ActiveLanguages)
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

        private void CreateDirectoryIfNotExists(string mediaPath)
        {
            if (!Directory.Exists(mediaPath))
            {
                Directory.CreateDirectory(mediaPath);
            }
        }

        private Dictionary<LanguageEnum, IFormFile> CreateFileDictionaryAllLanguages()
        {
            var files = new Dictionary<LanguageEnum, IFormFile>();
            foreach (var item in ActiveLanguages)
            {
                files.Add(item, null);
            }
            return files;
        }

        private IEnumerable<MediaTranslation> CreateMediaTranslationsAllLanguages()
        {
            foreach (var item in ActiveLanguages)
            {
                yield return new MediaTranslation() { LanguageId = item };
            }
        }
    }
}

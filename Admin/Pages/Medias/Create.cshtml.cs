using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Pages.Medias
{
    public class CreateModel : BaseMediaPageModel
    {
        private readonly IWebHostEnvironment _webHost;

        public CreateModel(IMediaService mediaService, IWebHostEnvironment webHost):base(mediaService)
        {
            _webHost = webHost;
        }

        [BindProperty]
        public Media Media { get; set; }
        [BindProperty]
        public Dictionary<LanguageEnum, IFormFile> Files { get; set; }
        public IEnumerable<LanguageEnum> ActiveLanguages => Enum.GetValues<LanguageEnum>();
        public async Task<IActionResult> OnGet()
        {
            Media = new Media
            {
                MediaTypeId = MediaTypeEnum.Photo,
                MediaTranslations = new List<MediaTranslation>(ActiveLanguages.Select(x => new MediaTranslation { LanguageId = x }))
            };

            Files = new(ActiveLanguages.Select(x => new KeyValuePair<LanguageEnum, IFormFile>(x, null)));

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!CheckAttachFiles())
            {
                ModelState.TryAddModelError("MediaTranslation_url", "At least one file must be selected and all selected files must be of the same type");
                return Page();
            }
            Media.MediaTypeId = ClassNameToMediaTypeId(GetClass());
            Media.AuthorId = GetCurrentUserId();
            Media.CreatedAt = DateTime.UtcNow;
            await ProcessingAttachFiles();
            await _mediaService.Upload(Media);
            return RedirectToPage("index");
        }

        private bool CheckAttachFiles()
        {
            if (Files.Values.All(x => x == null))
            {
                return false;
            }
            if (Files.Values
                .Where(x => x != null)
                .GroupBy(x => x.ContentType.Split("/").First())
                .Count() > 1)
            {
                return false;
            }
            return true;
        }

        string GetClass() 
        {
            var first = Files.FirstOrDefault(x => x.Value != null);
            if (first.Value == null)
                throw new Exception("Internal error");
            var mimeType = first.Value.ContentType;
            return mimeType.Split("/").First();
        }

        private async Task ProcessingAttachFiles()
        {
            var wwwPath = _webHost.WebRootPath;
            var classMedia = GetClass();
            var mediaPath = Path.Combine(wwwPath, ClassNameToDirectory(classMedia));
            CreateDirectoryIfNotExists(mediaPath);
            foreach (var item in ActiveLanguages)
            {
                if (Files[item] != null)
                {
                    var fileName = await GenerateNameFile(Files[item]);
                    var filePath = Path.Combine(mediaPath, fileName);
                    Media[item].Url = fileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        continue;
                    }
                    using var stream = new FileStream(filePath, FileMode.CreateNew);
                    await Files[item].CopyToAsync(stream);
                }
            }
            foreach (var item in Media.MediaTranslations.Where(x => string.IsNullOrEmpty(x.Url)))
            {
                item.Url = Media.MediaTranslations
                    .Where(x =>!string.IsNullOrEmpty(x.Url))
                    .Select(x=>x.Url)
                    .FirstOrDefault();
            }
        }

        private void CreateDirectoryIfNotExists(string mediaPath)
        {
            if (!Directory.Exists(mediaPath))
            {
                Directory.CreateDirectory(mediaPath);
            }
        }
    }
}

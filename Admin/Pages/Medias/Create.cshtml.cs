using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Pages.Medias
{
    public class CreateModel : BaseMediaPageModel
    {
        private readonly IWebHostEnvironment _webHost;

        public CreateModel(IMediaService mediaService, IWebHostEnvironment webHost) : base(mediaService)
        {
            _webHost = webHost;
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
            if (!CheckAttachFiles())
            {
                ModelState.TryAddModelError("MediaTranslation_url", "At least one file must be selected and all selected files must be of the same type");
                return Page();
            }
            Media.MediaType = ClassNameToMediaTypeId(GetClass());
            Media.AuthorId = GetCurrentUserId();

            await _mediaService.Create(Media);
            return RedirectToPage("index");
        }

        private bool CheckAttachFiles()
        {

            if (Media.Translations.All(x => x.File == null))
            {
                return false;
            }
            if (Media.Translations
                .Where(x => x.File != null)
                .GroupBy(x => x.File.ContentType.Split("/").First())
                .Count() > 1)
            {
                return false;
            }
            return true;
        }

        string GetClass()
        {
            var first = Media.Translations.FirstOrDefault(x => x.File != null);
            if (first == null)
                throw new Exception("Internal error");

            return first.File.ContentType.Split("/").First();
        }

        private async Task ProcessingAttachFiles()
        {
            var wwwPath = _webHost.WebRootPath;
            var classMedia = ClassNameToMediaTypeId(GetClass()).ToString();
            var mediaPath = Path.Combine(wwwPath, classMedia);
            CreateDirectoryIfNotExists(mediaPath);
            foreach (var item in Media.Translations)
            {
                if (item.File != null)
                {
                    var fileName = await GenerateNameFile(item.File);
                    var filePath = Path.Combine(mediaPath, fileName);
                    item.Url = fileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        continue;
                    }
                    using var stream = new FileStream(filePath, FileMode.CreateNew);
                    await item.File.CopyToAsync(stream);
                }
            }
            foreach (var item in Media.Translations.Where(x => string.IsNullOrEmpty(x.Url)))
            {
                item.Url = Media.Translations
                    .Where(x => !string.IsNullOrEmpty(x.Url))
                    .Select(x => x.Url)
                    .FirstOrDefault();
            }
        }

        private static void CreateDirectoryIfNotExists(string mediaPath)
        {
            if (!Directory.Exists(mediaPath))
            {
                Directory.CreateDirectory(mediaPath);
            }
        }
    }
}

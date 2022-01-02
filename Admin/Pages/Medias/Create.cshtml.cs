using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Admin.Pages.Medias
{
    public class CreateModel : BaseMediaPageModel
    {
        private readonly IWebHostEnvironment _webHost;

        public CreateModel(IMediaService mediaService, IWebHostEnvironment webHost) : base(mediaService)
        {
            _webHost = webHost;
        }

        public class MediaViewModel{
            public MediaViewModel()
            {

            }
            public MediaViewModel(string title, string description, string url, LanguageEnum languageId, IFormFile formFile)
            {
                Title = title;
                Description = description;
                Url = url;
                LanguageId = languageId;
                FormFile = formFile;
            }

            [MaxLength(128), Required]
            public string Title { get; set; }
            [Required]
            public string Description { get; set; }
            public string Url { get; set; }
            public LanguageEnum LanguageId { get; set; }
            public IFormFile FormFile { get; set; }
        }

        [BindProperty]
        public Dictionary<LanguageEnum, MediaViewModel> MediaData { get; set; }
        public async Task<IActionResult> OnGet()
        {
            MediaData = new(Settings.ActiveLanguages
                .Select(x => new KeyValuePair<LanguageEnum, MediaViewModel>(x,
                new MediaViewModel(string.Empty, string.Empty, string.Empty, x, null))));

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
            var media = new Media();
            media.MediaTypeId = ClassNameToMediaTypeId(GetClass());
            media.AuthorId = GetCurrentUserId();
            media.CreatedAt = DateTime.UtcNow;
            await ProcessingAttachFiles();
            media.MediaTranslations = MediaData.Values.Select(x => new MediaTranslation { 
                LanguageId = x.LanguageId,
                Title = x.Title,
                Description = x.Description,
                Url = x.Url,
            }).ToList();
            await _mediaService.Upload(media);
            return RedirectToPage("index");
        }

        private bool CheckAttachFiles()
        {

            if (MediaData.Values.All(x => x.FormFile == null))
            {
                return false;
            }
            if (MediaData.Values
                .Where(x => x.FormFile != null)
                .GroupBy(x => x.FormFile.ContentType.Split("/").First())
                .Count() > 1)
            {
                return false;
            }
            return true;
        }

        string GetClass()
        {
            var first = MediaData.FirstOrDefault(x => x.Value.FormFile != null);
            if (first.Value == null)
                throw new Exception("Internal error");
            var mimeType = first.Value.FormFile.ContentType;
            return mimeType.Split("/").First();
        }

        private async Task ProcessingAttachFiles()
        {
            var wwwPath = _webHost.WebRootPath;
            var classMedia = GetClass();
            var mediaPath = Path.Combine(wwwPath, ClassNameToDirectory(classMedia));
            CreateDirectoryIfNotExists(mediaPath);
            foreach (var item in Settings.ActiveLanguages)
            {
                if (MediaData[item].FormFile != null)
                {
                    var fileName = await GenerateNameFile(MediaData[item].FormFile);
                    var filePath = Path.Combine(mediaPath, fileName);
                    MediaData[item].Url = fileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        continue;
                    }
                    using var stream = new FileStream(filePath, FileMode.CreateNew);
                    await MediaData[item].FormFile.CopyToAsync(stream);
                }
            }
            foreach (var item in MediaData.Where(x => string.IsNullOrEmpty(x.Value.Url)))
            {
                item.Value.Url = MediaData
                    .Where(x => !string.IsNullOrEmpty(x.Value.Url))
                    .Select(x => x.Value.Url)
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

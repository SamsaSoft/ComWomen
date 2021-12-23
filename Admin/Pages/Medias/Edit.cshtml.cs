using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Pages.Medias
{
    public class EditModel : BaseMediaPageModel
    {
        private readonly IWebHostEnvironment _webHost;

        public EditModel(IMediaService mediaService, IWebHostEnvironment webHost) : base(mediaService)
        {
            _webHost = webHost;
        }
        [BindProperty]
        public Media Media { get; set; }
        [BindProperty]
        public Dictionary<LanguageEnum, IFormFile> Files { get; set; }

        public IEnumerable<LanguageEnum> ActiveLanguages => Media.MediaTranslations.Select(x => x.LanguageId);

        public IDictionary<string, string> MediaDirectoryDictionary => Enum
            .GetValues<MediaTypeEnum>()
            .Select(x => MediaTypeIdToClassName(x))
            .ToDictionary(x => x, x => ClassNameToDirectory(x));

        [BindProperty(SupportsGet = true)]
        public int MediaId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Media = await _mediaService.GetById(MediaId);
                Files = CreateFileDictionaryAllLanguages();
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
            Media.CreatedAt = DateTime.SpecifyKind(Media.CreatedAt, DateTimeKind.Utc); //fix db postgress
            Media.EditorId = GetCurrentUserId();
            Media.EditedAt = DateTime.UtcNow;
            await ProcessingAttachFiles();
            await _mediaService.Update(Media);
            return RedirectToPage("index");
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

        private async Task ProcessingAttachFiles()
        {
            var wwwPath = _webHost.WebRootPath;
            var classMedia = MediaTypeIdToClassName(Media.MediaTypeId);
            var mediaPath = Path.Combine(wwwPath, ClassNameToDirectory(classMedia));
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
        }
    }
}

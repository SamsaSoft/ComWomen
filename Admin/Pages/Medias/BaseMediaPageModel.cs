using Admin.Common;
using Core.Interfaces;

namespace Admin.Pages.Medias
{
    public class BaseMediaPageModel : BasePageModel
    {
        protected readonly IMediaService _mediaService;

        public BaseMediaPageModel(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }     

        public string MediaTypeIdToClassName(MediaTypeEnum mediaType) 
        {
            return mediaType switch
            {
                MediaTypeEnum.Photo => "image",
                MediaTypeEnum.Video => "video",
                MediaTypeEnum.Audio => "audio",
                _ => throw new ArgumentException("Not supported type"),
            };
        }

        public MediaTypeEnum ClassNameToMediaTypeId(string className) 
        {
            return className switch
            {
                "image" => MediaTypeEnum.Photo,
                "video" => MediaTypeEnum.Video,
                "audio" => MediaTypeEnum.Audio,
                _ => throw new ArgumentException("Not supported type"),
            };
        }

        public string ClassNameToDirectory(string className) 
        {
            return className switch
            {
                "image" => "images",
                "video" => "video",
                "audio" => "audio",
                _ => throw new ArgumentException("Not supported class"),
            };
        }
        protected async Task<string> GenerateNameFile(IFormFile formFile)
        {
            System.Security.Cryptography.HashAlgorithm hashAlg = System.Security.Cryptography.MD5.Create();
            var stream = formFile.OpenReadStream();
            var hash = await hashAlg.ComputeHashAsync(stream);
            var strhash = BitConverter.ToString(hash).Replace("-", "");
            return strhash + Path.GetExtension(formFile.FileName);
        }
    }
}

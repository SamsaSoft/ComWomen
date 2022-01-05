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

        public string MediaTypeIdToClassName(MediaType mediaType) 
        {
            return mediaType switch
            {
                MediaType.Photo => "image",
                MediaType.Video => "video",
                MediaType.Audio => "audio",
                _ => throw new ArgumentException("Not supported type"),
            };
        }

        public MediaType ClassNameToMediaTypeId(string className) 
        {
            return className switch
            {
                "image" => MediaType.Photo,
                "video" => MediaType.Video,
                "audio" => MediaType.Audio,
                _ => throw new ArgumentException("Not supported type"),
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

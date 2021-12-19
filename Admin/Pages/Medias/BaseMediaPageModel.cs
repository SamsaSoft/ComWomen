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

        public string LanguageIdToCode(LanguageEnum language)
        {
            return _mediaService.LanguageIdToCode(language);
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
    }
}

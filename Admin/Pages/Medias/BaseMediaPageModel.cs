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
    }
}

using Core.Enums;

namespace Core.DataAccess.Entities
{
    public class Language
    {
        public LanguageEnum Id { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public List<MediaTranslation> MediasTranslations { get; set; }
        public bool IsEnabled { get; set; }
    }
}

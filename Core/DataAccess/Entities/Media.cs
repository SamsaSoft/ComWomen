using Core.Enums;

namespace Core.DataAccess.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public MediaType MediaType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime? EditedAt { get; set; }
        public string EditorId { get; set; }
        public ApplicationUser Editor { get; set; }
        public List<MediaTranslation> Translations { get; set; }

        public MediaTranslation this[Language language] 
        {
            get { 
                var translation = Translations.FirstOrDefault(x => x.Language == language);
                if (translation == null)
                    return new MediaTranslation();
                return translation;
            }
        }
    }
}
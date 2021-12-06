using Core.Enums;

namespace Core.DataAccess.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public MediaTypeEnum MediaTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime? EditedAt { get; set; }
        public string EditorId { get; set; }
        public ApplicationUser Editor { get; set; }
        public List<MediaTranslation> MediaTranslations { get; set; }
    }
}
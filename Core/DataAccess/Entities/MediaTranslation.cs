using Core.Enums;
using System.ComponentModel.DataAnnotations;
namespace Core.DataAccess.Entities
{
    public class MediaTranslation
    {
        public int Id { get; set; }
        [MaxLength(128), Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Url { get; set; }
        public LanguageEnum LanguageId { get; set; }
        public DateTime? EditedAt { get; set; }
        public string EditorId { get; set; }
        public int? MediaId { get; set; }
        public Media Media { get; set; }
        public ApplicationUser Editor { get; set; }
    }
}
using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class MediaVM
    {
        public int Id { get; set; }
        public MediaTypeEnum MediaType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime? EditedAt { get; set; }
        public string EditorId { get; set; }
        public ApplicationUser Editor { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Url { get; set; }
        public LanguageEnum Language { get; set; }
    }
}

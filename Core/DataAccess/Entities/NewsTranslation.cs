using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.DataAccess.Entities
{
    public class NewsTranslation
    {
        public int Id { get; set; }
        [MaxLength(128), Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public List<Media> Media { get; set; }
        public Language Language { get; set; }

        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
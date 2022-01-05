using Core.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Language Language { get; set; }
        public int? MediaId { get; set; }
        public Media Media { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
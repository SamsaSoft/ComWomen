﻿using Core.Enums;
using System.ComponentModel.DataAnnotations;
namespace Core.DataAccess.Entities
{
    public class MediaTranslation
    {
        public int Id { get; set; }
        [MaxLength(128)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime? EditedAt { get; set; }
        public string EditorId { get; set; }
        public ApplicationUser Editor { get; set; }
    }
}
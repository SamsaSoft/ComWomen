﻿using Core.Enums;

namespace Core.DataAccess.Entities
{
    public class MediaTranslation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? MediaId { get; set; }
        public Media Media { get; set; }
        public LanguageEnum LanguageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime? EditedAt { get; set; }
        public string EditorId { get; set; }
        public ApplicationUser Editor { get; set; }
    }
}
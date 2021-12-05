using Core.Enums;

namespace Core.DataAccess.Entities
{
    public class MediaType
    {
        public MediaTypeEnum Id { get; set; }
        public string Name { get; set; }
        public List<Media> Medias { get; set; }
        public bool IsEnabled { get; set; }
    }
}

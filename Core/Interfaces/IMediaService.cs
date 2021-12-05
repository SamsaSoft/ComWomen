using Core.Enums;

namespace Core.Interfaces
{
    public interface IMediaService
    {
        Task DelteById(int id);
        Task<List<Media>> GetAllWithType(MediaTypeEnum type);
        Task<Media> GetById(int id);
        Task<Media> Upload(Media media);
    }
}
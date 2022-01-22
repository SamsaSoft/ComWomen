using Core.Enums;
using Core.Models;

namespace Core.Interfaces
{
    public interface IMediaService
    {
        /// <summary>
        /// Creates new media with its translations
        /// </summary>
        /// <param name="media">Media entity</param>
        /// <returns></returns>
        Task<OperationResult<int>> Create(Media media);
        Task<OperationResult<List<int>>> CreateList(List<Media> medias);
        Task DelteById(int id);
        Task<List<Media>> GetAllByType(MediaType type);
        Task<Media> GetById(int id);
        Task<Media> Upload(Media media);
        Task<OperationResult<int>> Update(Media media);
        Task<IEnumerable<Media>> GetAll();
    }
}
using Core.Enums;
using Core.Models;

namespace Core.Interfaces
{
    public interface INewsService
    {
        Task<OperationResult<int>> Create(News news);
        Task<OperationResult<News>> GetById(int id);
        Task<OperationResult<bool>> Update(News news);
        Task<OperationResult<bool>> DeleteById(int id);
        Task<List<News>> GetAllByLanguage(Language language);
    }
}

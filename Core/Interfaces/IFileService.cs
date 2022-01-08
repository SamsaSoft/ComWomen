using Core.Models;

namespace Core.Interfaces
{
    public interface IFileService
    {
        Task<OperationResult<int>> CreateFile(Media media);
        Task<OperationResult<int>> UpdateFile(Media media);
        Task<OperationResult<int>> DeleteFile(Media media);
    }
}

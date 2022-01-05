using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFileService
    {
        Task<OperationResult<int>> CreateFile(Media media);
        Task<OperationResult<int>> UpdateFile(Media media);
    }
}

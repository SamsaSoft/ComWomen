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
        Task<OperationResult<string>> CreateFile(Media media);
        Task<OperationResult<string>> UpdateFile(Media media);
    }
}

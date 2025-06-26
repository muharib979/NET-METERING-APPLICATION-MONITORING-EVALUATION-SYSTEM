using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{

    public interface IFileSaveRepository
    {
        Task<string> AddAsync(List<FileSaveDto> model);
    }
}

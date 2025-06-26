using Shared.DTOs.Agriculture;
using Shared.DTOs.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Agriculture.ServiceInterfaces
{
    public interface IAgricultureService: IBaseService<AgricultureDto>
    {
        Task<IEnumerable<AgricultureDto>> GetAllAsyncByDate(string validDate);
        Task<IEnumerable<AgricultureDto>> GetAllAgricultureAsyncByDate(string validDate);
        Task<IEnumerable<AgricultureDto>> GetAllPoultryAsyncByDate(string validDate);
    }
}

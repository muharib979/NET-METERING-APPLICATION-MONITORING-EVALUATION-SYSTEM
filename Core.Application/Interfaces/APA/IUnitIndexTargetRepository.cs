using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.APA
{
    public interface IUnitIndexTargetRepository
    {
        Task<List<UnitIndexAndPerformValueDto>> GetAllUnitIndexAndPerformValueByIndexCode(string indexCode);
        Task<List<TargetDto>> GetAllTargetDataByPerformanceCode(string performCode);
    }
}

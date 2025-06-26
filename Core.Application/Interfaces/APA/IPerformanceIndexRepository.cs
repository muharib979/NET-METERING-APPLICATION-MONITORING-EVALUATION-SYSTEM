using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.APA
{
    public interface IPerformanceIndexRepository
    {
        Task<List<PerfomanceIndexDto>> GetAllPerformanceIndexDataByProgramCode(string programCode);
        Task<List<PerfomanceIndexDto>> GetAllPerformanceIndexList();
        Task<bool> SavePerformanceBill(PerfomanceIndexDto model);
        Task<int> DeletePerformanceBill(int id);
    }
}

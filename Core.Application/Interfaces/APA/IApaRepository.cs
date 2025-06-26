using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.APA
{
    public interface IApaRepository
    {
        Task<bool> SaveApaData(List<ApaDTO> model);
        Task<List<ApaDTO>> GetApaByMonth(string billMonth);
        Task<ApaUitDTO> GetApaUnitIndexTarget(string performanceCode);
        Task<List<ApaDTO>> GetAllApaData();
        Task<List<object>> GetAPAReportByBillMonth(string fromBillMonth, string toBillMonth);
    }
}

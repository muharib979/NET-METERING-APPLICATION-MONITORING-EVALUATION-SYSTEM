using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.APA
{
    public interface ITargetRepository
    {
        Task<List<TargetDTO>> GetTargetList();
        Task<List<FinancialYearDTO>> GetFinancialYearList();
        Task<bool> SaveTargetBill(TargetDTO model);
        Task<int> DeleteTargetBill(int id);
    }
}

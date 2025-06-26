using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IInstallmentPlanRepository<T> where T : class
    {
        Task<bool> SavePenaltyInstallment(List<T> model);
        Task<List<T>> GetPenaltyInstallment(string billNumber);
    }
}

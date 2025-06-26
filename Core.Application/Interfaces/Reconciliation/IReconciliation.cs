using Core.Domain.Reconciliation;
using Shared.DTOs.Reconciliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Reconciliation
{
    public interface IReconciliation
    {
        Task<List<ReconcilationStatusDTO>> GetConsumerReconcilation(string startDate, string endDate,string user);
        Task<bool> SaveConsumerReconciliation(ReconcilationStatusDTO model);

        Task<List<ReconcilationStatusDTO>> GetMiscReconcilation(string startDate, string endDate,string user);

        Task<bool> SaveMiscReconciliation(ReconcilationStatusDTO model);

    }
}

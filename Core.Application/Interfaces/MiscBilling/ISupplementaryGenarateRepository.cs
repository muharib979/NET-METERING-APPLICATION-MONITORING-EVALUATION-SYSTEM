using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface ISupplementaryGenarateRepository
    {
        Task<ReturnDto> SaveSupplenmentaryBill(SupplementaryBillDTO model);
        Task<List<SupplementaryBillViewDTO>> GetSupplementaryView( SupplementaryBillViewDTO model);
        Task<bool> SaveSupplementaryExistingConsumer(SupplementaryExistingConsumerDTO model);
    }
}

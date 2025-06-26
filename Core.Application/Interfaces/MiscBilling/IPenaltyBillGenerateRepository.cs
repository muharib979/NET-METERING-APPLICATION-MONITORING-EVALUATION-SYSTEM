using Core.Domain.MISCBILL;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IPenaltyBillGenerateRepository
    {
        Task<ReturnDTO> SavePenaltyBill(PenaltyBillDTO model);
        Task<List<PenaltyBillViewDTO>> GetPenaltyView(PenaltyBillViewDTO model);
        Task<Returndto> SaveNonConsumerBill(NonConsumerPenaltyBillDTO model);
        Task<List<NonConsumerPenaltyBillViewDTO>>GetNonConsumerView(NonConsumerPenaltyBillViewDTO model);
        Task<bool> SavePenaltyExistingConsumer(PenaltyExistingBillDto model);
        Task<ReturnDTO> SavePenaltyBillPrepaid(PenaltyBillPrepaidDTO modal);
        Task<List<PenaltyBillPrtepaidViewDTO>> GetPenaltyBillPrepaidView(PenaltyBillPrtepaidViewDTO modal);

    }
}

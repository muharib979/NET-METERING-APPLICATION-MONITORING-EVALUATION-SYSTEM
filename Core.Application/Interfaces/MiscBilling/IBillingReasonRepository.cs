using Shared.DTOs.CustomerDto;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IBillingReasonRepository
    {
        Task<List<BillReasonDTO>> GetBillingReason();
        Task<List<BillReasonDTO>> GetDCRCBillingReason();
        Task<List<BillReasonDTO>> GetBillReasonForPenalty();
        Task<List<BillReasonDTO>> GetBillReasonForPenaltyNonCust();
        Task<List<BillReasonDTO>> GetBillReasonForSupplementaryBill();
        Task<List<BillReasonDTO>> GetBillReasonForMiscChargeBill();
    }
}

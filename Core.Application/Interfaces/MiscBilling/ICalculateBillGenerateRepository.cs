using Core.Domain.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface ICalculateBillGenerateRepository
    {
        Task<CalculateBills> GetCalculatePenaltyBill(CalculateBills calculateBills);
        Task<CalculateBills>GetCalculateSupplementaryBill(CalculateBills calculateBills);
        Task<CalculateBills>GetCalculateDcRcBill(CalculateBills calculateBills);
        Task<CalculateBills> GetCalculateNonConsumerpenaltyBill(CalculateBills calculateBills);
        Task<CalculateBills> GetCalculateMiscChargeBill(CalculateBills calculateBills);
    }
}

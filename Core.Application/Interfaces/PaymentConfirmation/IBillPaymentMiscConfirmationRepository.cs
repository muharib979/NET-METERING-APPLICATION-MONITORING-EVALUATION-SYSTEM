using Shared.DTOs.CustomerDto;
using Shared.DTOs.PaymentConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.PaymentConfirmation
{
    public interface IBillPaymentMiscConfirmationRepository
    {
        Task<BillPaymentMiscConfirmationDto> BillPaymentMiscConfirmation(string billNumber,bool isPaid, DateTime paymentDate);
        Task<BillPaymentMiscStatusDto> BillPaymentMiscStatus(string billNumber);
    }
}

using Shared.DTOs.CustomerDto;
using Shared.DTOs.PaymentConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.PaymentConfirmation
{
    public interface IPaymentConfirmationRepository
    {
        Task<PaymentConfirmationDto> PaymentConfirmation(string billNumber, string locationCode, bool isPaid, DateTime paymentDate);
        Task<PaymentStatusDto> PaymentStatus(string billNumber,string locationCode);
    }
}

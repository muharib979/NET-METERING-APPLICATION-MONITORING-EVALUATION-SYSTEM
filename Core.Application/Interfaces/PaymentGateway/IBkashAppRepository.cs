using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.PaymentGateway;

public interface IBkashAppRepository
{
    Task<BkashCheckBillResponseDTO> CheckBillBkash(string userName, string password, string customerNo, string? billMonth);
    Task<BkashCallBackResponseDTO> SaveBkashPaymentFromApp(string userName, string password, string customerNo, string? billMonth,string amount,string mobileNumber,string transactionId,string payTime);
}

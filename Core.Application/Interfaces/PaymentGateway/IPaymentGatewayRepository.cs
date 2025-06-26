using Core.Domain.PaymnetGateway;
using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.PaymentGateway
{
    public interface IPaymentGatewayRepository
    {
        Task<bool> SavePaymentGateway(PaymentReceivedDataDTO model);

        Task<List<RequestData>> GetBillInformation(string customerNumber, string billNumber);
        Task<bool> SaveRequestData(RequestData model);
        Task<List<RequestData>> GetNagadBillInformation(string customerNumber, string billNumber);
        Task<bool> SaveNagadRequestData(RequestData model, string orderId);

    }
}

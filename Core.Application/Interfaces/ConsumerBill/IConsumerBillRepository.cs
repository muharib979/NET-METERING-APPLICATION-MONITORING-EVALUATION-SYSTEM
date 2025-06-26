using Shared.DTOs.ConsumerBill;
using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ConsumerBill
{
    public interface IConsumerBillRepository
    {
        Task<List<ConsumerBillDTO>> BillDetailsByBillNumber(string customerNumber, string billNumber, string user);
        Task<List<Core.Domain.MISCBILL.ConsumerBill>> GetConsumerData(string customerNumber, string billNumber);
        Task<bool> SaveConsumerRequestData(Core.Domain.MISCBILL.ConsumerBill model);
        Task<bool> SaveConsumerPayment(PaymentReceivedDataDTO model);


    }
}

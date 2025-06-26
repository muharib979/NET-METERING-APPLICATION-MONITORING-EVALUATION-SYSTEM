using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface ICustomerDetailsRepository
    {
        Task<CustomerDetailsDTO> GetCustomerByCustomerNymber(string custNumber, string locationCode);
        Task<CustomerDetailsDTO> GetCustomerByBillNumber(string billNumber, string custNumber);
        Task<CustomerDetailsDTO> GetNonCustomerByBillNumber(string billNumber, string custNumber);
        Task<CustomerDetailsDTO> GetUntracedCustomer(string custNumber, string dbCode, string locCode);
        Task<CustomerDetailsPenaltyBillPrepaidDTO> GetCustomerDetailsPrepaidBillPenalty(string custNumber, string dbCode, string locCode);
        Task<UserInfoDto> GetCustomersInformation(string customerNumber, string dbCode, string locCode);
    }
}

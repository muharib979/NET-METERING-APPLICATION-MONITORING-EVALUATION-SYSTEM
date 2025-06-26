using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IUntracedConsumerRepository
    {
        Task<bool> SaveUntracedConsumer(UntracedConsumerDTO model);
        Task<UntracedConsumerDTO> GetConsumerByCustNumber(string custNumber);
        Task<bool> ChangeConsumerStatus(int id, int custStatus,string updatedBy);
        Task<List<UntracedConsumerDTO>> GetAllConsumer();
        Task<List<UntracedCustomerViewDTO>> GetConsumerByDate(string startDate, string endDate, string locationCode);

        Task<List<UntracedConsumerDTO>> GetConsumerSearchByDate(string startDate, string endDate, string locationCode);

    }
}

using Core.Domain.MISCBILL;
using Core.Domain.Temporary;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface ITemporaryBillRepository
    {
        Task<List<FeederDTO>> GetAllFeeder(string locationCode);
        Task<List<InitialReadingDTO>> GetInitialReading(string meterTypeCode);
        Task<bool> SaveCensusBill(CensusBillDTO model);
        Task<List<CustomerCensusList>> GetCensusCustomerList(string locationCode);
        Task<List<TemporaryBillViewDTO>> GetTemporaryBillView(TemporaryBillViewDTO model);


    }
}

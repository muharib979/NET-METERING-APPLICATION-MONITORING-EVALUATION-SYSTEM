using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IMrsGenarateRepository
    {
        Task<List<MRSGenarateDTO>> GetMRSGenarate(string locationCode, string customerNumber, string billCycleCode);
        Task<bool> SaveMrsGenerate(List<MRSGenarateDTO> model);
        Task<List<MRSBillCalculationDTO>> GetMrsBillCalculation(string userLocation, string customerNumber, string billCycleCode);
        Task<MRSBillPrintDTO> GetMrsBillReport(string locationCode, string customerNumber, string billNumber);
    }
}

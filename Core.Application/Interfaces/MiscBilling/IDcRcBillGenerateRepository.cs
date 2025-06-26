using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IDcRcBillGenerateRepository
    {
        Task<List<DcTypeDTO>> GetAllDcType();
        Task<ReturnDTO> SaveDcRcBill(DcRcBillDTO model);
        Task<List<DcRcBillViewDTO>> GetDcRcView(DcRcBillViewDTO model);
        Task<bool> UpdateRcDate(string billNumber,string customerNumber,string rcDate, string userName);

    }
}

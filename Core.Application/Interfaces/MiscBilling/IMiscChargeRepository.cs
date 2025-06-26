using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IMiscChargeRepository
    {
        Task<List<DcTypeDTO>> GetAllDcType();
        Task<ReturnDTO> SaveMiscChargeBill(MiscChargeDTO model);
        Task<List<MiscChargeViewDTO>> GetMiscView(MiscChargeViewDTO model);
        Task<bool> UpdateMiscDate(string billNumber, string customerNumber, string rcDate);
    }
}

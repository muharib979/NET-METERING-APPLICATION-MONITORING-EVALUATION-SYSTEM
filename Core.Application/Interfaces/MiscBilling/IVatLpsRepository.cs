using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IVatLpsRepository
    {
        Task<VatLpsDTO> GetPenaltyVatLps(string customerNumber, string locationCode);
        Task<VatLpsDTO> GetSupplementaryVatLps(string customerNumber, string locationCode);
        Task<VatLpsDTO> GetDcRcVatLps(string customerNumber, string locationCode);
    }
}

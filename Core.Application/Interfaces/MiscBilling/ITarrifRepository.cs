using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface ITarrifRepository
    {
        Task<List<TariffDTO>> GetAllTariff();
        Task<List<TariffDTO>> GetAllNonCustTariff(string locationCode);
    }
}

using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface ILocation
    {
        Task<List<LocationDTO>> GetNonCustLocationDesc(string locationCode);
    }
}

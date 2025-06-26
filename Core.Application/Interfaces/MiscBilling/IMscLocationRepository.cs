using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IMscLocationRepository
    {
        Task<List<LocationDDDTO>> GetLocationAsync();
        Task<List<LocationsDTO>> GetLocationByUser(string locationCode);
        Task<List<BlockNumDTO>> GetBlockNumByLocation(string locationCode);
    }
}

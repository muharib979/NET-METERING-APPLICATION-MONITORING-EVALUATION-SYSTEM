using Core.Domain.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IMeterRepository
    {
        Task<List<MeterOwnerDTO>> GetAllMeterOwner();
        Task<List<MeterTypeDTO>> GetAllMeterType();
        Task<List<MeterCondionDTO>> GetAllMeterCondition();
    }
}

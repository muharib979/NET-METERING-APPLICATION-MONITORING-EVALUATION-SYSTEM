using Core.Domain.Police;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Police
{
    public interface IPoliceRepository
    {
        Task<List<OnlinePoliceDetailsDTO>> OnlinePoliceDetails(string billMonth, string zoneCode, string locationCode, string reportType);

    }
}

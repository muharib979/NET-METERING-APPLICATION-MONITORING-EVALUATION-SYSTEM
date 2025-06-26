using Core.Application.Common.Interfaces;
using Core.Domain.ZoneCircle;
using Shared.DTOs.DatabaseConfig;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ZoneCircle
{
    public interface IZoneCircleRepository
    {
        Task<List<Domain.ZoneCircle.Zone>> GetAllZone();
        Task<List<Circle>> GetAllCircle();
        Task<List<Circle>> GetAllCircleByZoneCode(string zoneCode);
        Task<List<ZoneDto>> GetZoneByUserID(string userName);
        Task<List<CircleDTO>> GetCircleByUserNameZoneCode(string userName, string zoneCode);
        Task<List<DatabaseConfigDto>> GetDBCenterByUserName(string userName);
    }
}

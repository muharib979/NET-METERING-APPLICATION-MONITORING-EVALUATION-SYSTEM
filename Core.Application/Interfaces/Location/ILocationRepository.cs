using Core.Domain.Location;
using Shared.DTOs.Location;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Location
{
    public interface ILocationRepository : IBaseRepository<LocationsDto>
    {
        Task<List<Core.Domain.Location.Locations>> GetLocationBydbcode(string dbCode);
        Task<List<LocationsDto>> GetLocationByDbArray(string[] dbCode);
        Task<List<LocationDTO>> GetNonCustLocationDesc(string locationCode);
        Task<Locations> GetLocationByLocation(string locationCode);
        Task<List<LocationsDto>> GetLocationByZoneCode(string zoneCode);
        Task<List<Locations>> GetLocationByDBMIdAsync(string dbCode, int userId, int RoleId = 0);
        Task<List<Locations>> GetLocationByMultipleDBUserRoleId(string[] dbCodes, int userId, int RoleId = 0);
        Task<List<LocationsDto>> GetLocationAndDeptCode();
        Task<List<ZoneCircleLocationDTO>> GetZoneCircleByLocation(string locationCode);
        Task<List<DropdownResultForStringKey>> GetAllPrepaidDivision();
        Task<List<DropdownResultForStringKey>> GetPrepaidDistrictByDivision(string divCode);
        Task<List<DropdownResultForStringKey>> GetPrepaidThanaByDistrict(string distCode);
        Task<List<LocationsDto>> GetLocationByUserNameCircleCode(string userName, string circleCode);
        Task<List<LocationsDto>> GetLocationByUserNameZoneCode(string userName, string zoneCode);
        Task<List<LocationsDto>> GetLocationByLocCode(string locCode);
        Task<LocationsDto> GetDivisionAndDistrictByLocation(string locCode);
        Task<List<LocationsDto>> GetLocationByUserNameCenterCode(string userName, string[] db);
        Task<List<LocationsDto>> GetLocationAndDeptCodeByCenterCode(string centerCode);

    }
}

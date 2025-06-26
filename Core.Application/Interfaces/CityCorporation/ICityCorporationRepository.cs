using Core.Domain.CityCorporation;
using Shared.DTOs.CityCorporation;

namespace Core.Application.Interfaces.CityCorporation
{
    public interface ICityCorporationRepository
    {
        Task<List<CityCorporationDto>> GetPourAndCityCorporbyDate(string billMonth, string reportType, string zoneCode, string locationCode,string circleCode);
        Task<List<CityCorporationDataDto>> GetAllCityCorporation();
        Task<List<ZoneWiseCityPouroUnionArrear>> GetZoneWiseCityPouroUnionArrear(string billMonth);
        Task<List<ZoneLocationWiseCityPouroUnion>> GetZoneLocationWiseCityPouroUnionArrear(string zoneCode,string billMonth);
        Task<List<CityCorporationDetailsDTO>> GetAllCityCorporAndPouroshovaArrearDetails(string billMonth, string reportType, string zoneCode, string locationCode, string circleCode);
        Task<List<OnlineCityPourMergeData>> GetAllOnlinePouroAndCityCorporBn(string billMonth, string zoneCode, string locationCode);
        Task<List<OnlineCityPourMergeData>> GetOnlinePouroAndCityCodeWiseBn(string billMonth, string zoneCode, string locationCode, string cityCorporationCode, string pouroshovaCode);
        Task<List<OnlineCityPourMergeData>> GetAllOnlinePouroAndCityCorpoBnDetails(string billMonth, string zoneCode, string locationCode);
        Task<List<OnlineCityPourMergeData>> GetOnlinePouroAndCityCodeWiseBnDetails(string billMonth, string zoneCode, string locationCode, string cityCorporationCode, string pouroshovaCode);
        Task<List<ZoneWiseCitycorporationDataDTO>> GetZoneWiseCitycorporationData(string zoneCode);
        Task<List<ZoneWisePouroshovaDataDTO>> GetZoneWisePouroshovaData(string zoneCode);
        Task<List<OnlineCityPourMergeData>> GetOnlinePouroAndCityDetailsWithCRV(string billMonth, string zoneCode, string locationCode, string reportType);
    }
}

using Shared.DTOs.Location;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Religious
{
    public interface IReligiousRepository
    {
        Task<List<ReligiousDTOs>> GetAllReligiousSummaryData(string reportDate, string zoneCode, string locationCode, string reportType, string circleCode);
        Task<List<LocationsDto>> GetLocationByCircle(string circleCode);
        Task<List<NoOfMosqueAndOtherPlaceOfWorshipDTO>> GetNoOfMosqueAndWorshipData(string startMonth, string endMonth);
        Task<List<LocationWiseReligiousRptDTO>> GetLocationWiseRpt(string? zoneCode, string? locationCode, string startMonth, string endMonth);
        Task<List<ReligiousArrearSummaryDTO>> GetReligiousArrearSummary();
        Task<List<ReligiousReceiptDTO>> GetReligiousReceiptList( string locationCode);
        Task<List<LocationWiseReligiousRptDTO>> GetReligiousByMonth(string billMonth, string zoneCode, string locationCode);

        Task<bool> SaveReligiousReceiptBill(ReligiousReceiptViewModel model);
    }
}

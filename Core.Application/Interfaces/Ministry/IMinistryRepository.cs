using Core.Domain.Ministry;
using Shared.DTOs.Ministry;
using Shared.DTOs.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Ministry
{
    public interface IMinistryRepository
    {
        Task<List<MinistryArrear>> GetAllMinistrybyCurrDate(string currDate);
        Task<List<MinistryDepartment>> GetAllMinistryDept();
        Task<List<MinistryData>> GetAllMinistry();
        Task<List<MinistryDetails>> GetMinistryAndDateWiseMinistryDetails(string code, bool? isRebate, string billMonth, string zoneCode, string locationCode, string circleCode, string reportType);
        //Task<List<MinistryDetails>> GetAllMinistryDetails();
        Task<List<MinistrySummary>> GetMinistrySummaryByDate(string zoneCode, string reportDate, string? locationCode);
        Task<List<GetCustomerArrearModel>> GetOldMinistryDetails(string billMonth, string ministryCode, int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string dbCode, string locationCode);
        Task<List<ViewMinistryDTO>> GetMinistryList();
        Task<bool> SaveMinistryBill(ViewMinistryDTO model);
        Task<List<MinistryLedgerDTO>> GetMinistryLedgerByDate(string billMonth);
        Task<List<PoliceArrearDetailsDTO>> GetPoliceArrearReportDetails(string zoneCode, string reportDate, string? locationCode, string reportType);
        Task<List<PoliceArrearSummaryDTO>> GetPoliceArrearReportSummary(string zoneCode, string reportDate, string? locationCode, string reportType);
        Task<List<RailwayDTO>> GetRailwayDetails(string zoneCode, string reportDate, string? locationCode, string reportType, string circleCode);
        Task<List<RailwaySummaryDTO>> GetRailwaySummary(string zoneCode, string reportDate, string? locationCode, string reportType, string circleCode);
        Task<List<GetCustomerArrearModel>> GetOnlineMinistryZoneWiseDetails(string billMonth, string ministryCode, int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string zoneCode, string locationCode, bool isRebate);
        Task<List<MinistryDetailsSummaryMergeDTO>> GetOnlineMinistryZoneWiseSummary(string billMonth, string ministryCode, int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string zoneCode, string locationCode, bool isRebate);
        Task<List<LocationWiseArrearDTO>> GetLocationWiseMinistryArrear(string zoneCode, string circleCode, string? locationCode, string billMonth);
        Task<List<LocationWiseArrearDTO>> GetTotalLocationWiseMinistryArrear(string zoneCode, string circleCode, string? locationCode, string billMonth);
        Task<List<LocationWiseArrearDTO>> GetTotalMinistryArrear(string zoneCode, string circleCode, string? locationCode, string billMonth);
        Task<List<MinistryArrearUpToDateMergeDataDTO>> OnlineMinistyArrearUpToDateInfo(string billMonth, string zoneCode, string locationCode);
        Task<List<OnlineMinistryArrearDetailsMergeDTO>> OnlineMinistyArrearDetails(string billMonth, string ministryCode, string zoneCode, string locationCode, string reportType);
        Task<List<OnlineMinistryArrearDetailsMergeDTO>> OnlineMinistyArrearDetailsWithCRV(string billMonth, string ministryCode, string zoneCode, string locationCode, string reportType);
        Task<List<OnlineMinistryArrearDetailsMergeDTO>> OnlineMinistyArrearDetailsByCode(string billMonth, string ministryCode, string zoneCode, string locationCode, string reportType);
        Task<List<PreModDataDTO>> GetPreModDataByBillMonth(string billMonth, string deptCode);
        Task<List<PouroshovaDTO>> GetAllPouroshova();
        Task<bool> SavePouroshovaBill(PouroshovaDTO model);
        Task<int> DeletePouroshovaBill(int id);

        Task<List<UnionPorishodDTO>> GetAllUnionPorishod();
        Task<bool> SaveUnionPorishodBill(UnionPorishodDTO model);

        Task<int> DeleteUnionBill(int id);

        Task<List<OnlineMinistryArrearDetailsMergeDTO>> GetOnlineMinistryArrearByBillMonth(string zoneCode,string locationCode,string billMonth);
        Task<List<OnlineMinistryArrearDetailsMergeDTO>> GetOnlineMinistryArrearSummaryWithCRV(string zoneCode, string locationCode, string billMonth);
        Task<List<OnlineMinistryArrearDetailsMergeDTO>> GetOnlineMinistryArrearDetailsWithCRV(string billMonth, string ministryCode, string zoneCode, string locationCode, string reportType);
        Task<AllCustomerMode> GetAllCustomerDetails(string billMonth, int NoOfMonth, int ArrrearFrom, int ArrrearTo, bool isAll, bool isPrincipal, bool isLPS, bool isVAT, string dbCode, string locationCode);
        
    }
}

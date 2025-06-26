using Core.Domain.Location;
using Core.Domain.MISReport;
using Shared.DTOs.MISReport;

namespace Core.Application.Interfaces.MISReport
{
    public interface IMisReportRepository
    {
        Task<List<CustomerArrear>> AllCustomerArrearDetails(string[] dbCodes, string[] locationCodes, int fromAmount, int toAmount, string tariff, string billGroupId, string bookId, int noOfMonth, string billMonth, int reportTypeId, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, string orderTypeId, int userId, int roleId);
        Task<List<AllCustoemrArrearSummary>> GetAllCustomerArrearLocationSummaryData(string dbCode, string billMonth, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, int userId, int roleId);
        Task<List<AllCustoemrArrearSummary>> AllCustomerArrearSummaryCenterWise(string billCycleCode, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, int userId, int roleId);
        Task<List<RegularCustomerArrear>> GetArrearRegularCustomerData(int[] dbIds, int[] locationIds, string BillMonth, int ConnStatusId, int ArrearFrom, int ArrrearTo, int TariffId, string Tariff, string BillGroupId, string BookId, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, string OrderTypeId, int UId, int roleId);
        Task<Locations> GetLocationByIDAsync(int id);
        Task<List<CenterWiseArrearSummaryDTO>> GetArrearLocationSummaryInfoAsync(string CenterCode, string BillMonth, bool isAll, bool isPrincipal, bool isVat, bool isLPS, int userId, int roleId);
        Task<List<CenterWiseArrearSummaryDTO>> GetCenterWiseArrearSummary(string billCycleCode, bool isAll, bool isPrincipal, bool isVat, bool isLPS, int userId, int roleId);
        Task<List<PrepaidCustomerArrearBasedOnOffsetDTO>> PrepaidCustomerArrarBasedOnOffset(string[] dbCodes, string[]? locationCodes, string consumerNo, string tariff, decimal fromAmount, decimal toAmount, int status, bool isAll, bool isPrincipal, bool isVat, bool isLps, string orderBy, string reportType, int userId, int roleId = 0);
        Task<List<PrepaidCustomerArrearBasedOnOffsetDTO>> PrepaidCustomerArrarBasedOnOffsetSummary(string[] dbCodes, string[]? locationCodes, string consumerNo, string tariff, decimal fromAmount, decimal toAmount, int status, bool isAll, bool isPrincipal, bool isVat, bool isLps, string orderBy, string reportType, int userId, int roleId = 0);
        Task<List<IllegalConsumerPenaltyDTO>> GetIllegalConsumerPenalty( string billMonth);
    }
}

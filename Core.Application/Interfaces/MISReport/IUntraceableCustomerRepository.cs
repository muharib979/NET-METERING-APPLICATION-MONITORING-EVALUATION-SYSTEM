using Core.Domain.Untracable;
using Shared.DTOs.MISReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MISReport
{
    public interface IUntraceableCustomerRepository
    {
        Task<List<MergeUntraceable>> GetAllIUntraceableCustomer(string billMonth);
        //Task<List<UntracebleCustArrearReportDTO>> GetUntracedCustArrearCollection(string zoneCode, string circleCode, string locationCode, string billMonth);
        Task<Tuple<List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>>> GetUntracedArrearDetails(string zoneCode, string circleCode, string locationCode, string billMonth);
        Task<Tuple<List<UntracedCustomerArrearModel>, List<UntracedCustomerArrearModel>, List<UntracedCustArrearMergeSummaryDto>, List<UntracedCustArrearMergeSummaryDto>>> GetUntracedArrearSummary(string zoneCode, string billMonth);
        Task<List<UntracebleCustReportDTO>> GetMisUntraceableCustomer(string[] dbCodes, string[] locationodes, string billMonth, int userId, int roleId);
        Task<List<Untraceable>> GetUCTraceDataCurrMonthWithZone(string zoneCode, string billMonth);
        Task<List<UntracebleCustArrearDetailsReportDTO>> GetUntraceableArrearDetails(string zoneCode, string circleCode, string locationCode, string billMonth);
    }
}

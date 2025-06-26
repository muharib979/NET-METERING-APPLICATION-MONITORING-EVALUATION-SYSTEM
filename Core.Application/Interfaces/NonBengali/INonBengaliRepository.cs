using Core.Domain.NonBengali;
using Shared.DTOs.NonBengali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.NonBengali
{
    public interface INonBengaliRepository
    {
        Task<List<NonBengaliDTOs>> GetNonBengaliConsumerByDate(string validDate,string reportType);
        Task<List<NonBengaliSummaryDTO>> GetNonBengaliConsumerSummaryByDate(string validDate,string reportType);
        Task<List<OnlineNonBengaliSummaryMergeDTO>> GetOnlineNonBengaliConsumerDataByDate(string zoneCode, string locationCode,string billMonth, string reportType);
        Task<List<NonBengaliSummaryDTO>> GetNonBengaliStaticValue(string zoneCode, string locationCode);

    }
}

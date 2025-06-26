using DocumentFormat.OpenXml.VariantTypes;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface ILogEventRepository
    {
        Task<List<LogEventDTO>> GetLogEvent();
        Task<List<LogEventDTO>> GetLogEventByDate(string fromDate, string toDate);
        Task<int> DeleteLogEventByDate(string fromDate, string toDate);
        Task<int> DeleteLogEvent(int id);
    }
}

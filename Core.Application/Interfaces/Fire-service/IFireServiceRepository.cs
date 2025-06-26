using Core.Domain.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IFireServiceRepository
    {
        Task<List<PublicSecurityDivisionMergeDTO>> OnlineSecurityServiceDivArrearDetails(string billMonth, string zoneCode, string locationCode, string reportType);

    }
}

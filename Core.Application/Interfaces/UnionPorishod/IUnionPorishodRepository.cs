using Core.Domain.UnionPorishad;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.UnionPorishod
{
    public interface IUnionPorishodRepository
    {
        Task<List<ZoneWiseUnionPorishodDto>> GetUnionPorishodbyDate(string zoneCode,string circleCode, string locationCode, string billMonth, string reportType);
        Task<List<OnlineUnionPorisadMergeDataDto>> GetOnlineUnionPorishod(string zoneCode, string locationCode, string billMonth,string reportType);
    }
}

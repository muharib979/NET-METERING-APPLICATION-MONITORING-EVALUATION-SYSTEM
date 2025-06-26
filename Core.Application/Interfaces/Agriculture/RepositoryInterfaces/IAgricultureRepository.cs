
using Shared.DTOs.Agriculture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Application.Interfaces.Agriculture.RepositoryInterfaces
{
    public interface IAgricultureRepository: IBaseRepository<AgricultureDto>
    {
        Task<List<AgricultureDto>> GetAllAsyncByDate(string validDate);
        Task<List<AgricultureDto>> GetAllAgricultureAsyncByDate(string validDate);
        Task<List<AgricultureDto>> GetAllPoultryAsyncByDate(string validDate);
        Task<List<OnlineAgricultureMergeDTO>> GetOnlineAgriPoultryAndMinistryData(string billMonth, string zoneCode, string locationCode, string reportType);
        Task<List<OnlineAgricultureLedgerMergeDTO>> GetOnlineAgriAndPoultryLedger(string startMonth, string endMonth, string zoneCode, string locationCode);
    }
}

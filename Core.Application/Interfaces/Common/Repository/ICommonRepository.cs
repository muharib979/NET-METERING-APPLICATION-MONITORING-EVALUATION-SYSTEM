using Core.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Common.Repository
{
    public interface ICommonRepository
    {
        string CreateConnectionString(string host, string port, string serviceName, string userId, string password);
        Task<string> GetServiceNameByDBId(int dbId);
        Task<string> GetDBNameByDBId(int dbId);
        Task<string> GetConnectionStringByDBId(int dbId);
        Task<string> GetMaxBillCycleCode(string connectionString);
        Task<List<BillGroup>> getLocationWiseBillGroupData(string[] dbCodes, string[] locationCode);
        Task<List<BillGroup>> getDatabaseWiseBillGroupData(string[] dbCode);
        Task<List<Book>> getMultipleLocationWiseBookData(string[] dbCodes, string[] locationCodes, string billGroupId);
        Task<List<Book>> getMultipleDatabaseWiseBookData(string[] dBCodes, string billGroupId);
        Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDBByUserIdAsync(int userId, int RoleId);
        Task<object> GetLocationByDBMIdAsync(string dbCode, int userId, int RoleId = 0);
    }
}

using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Core.Domain.OfficeStuff;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class LogEventRepository : ILogEventRepository
    {
        public async Task<int> DeleteLogEvent(int id)
        {
            try
            {
                using var con = new OracleConnection(Connection.ConnectionString());
                string sQuery = "Delete  FROM LOG WHERE \"Id\" = :Id";
                var result = await con.ExecuteAsync(sQuery, new { Id = id });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteLogEventByDate(string fromDate, string toDate)
        {
            try
            {
                using var con = new OracleConnection(Connection.ConnectionString());
                string sQuery = @"Delete From LOG where " + "\"TimeStamp\"" + @" between :FromDate And :ToDate";
                var result = await con.ExecuteAsync(sQuery, new { FromDate = fromDate.ToUpper(), ToDate = toDate.ToUpper() });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LogEventDTO>> GetLogEvent()
        {
            try
            {
                using var con = new OracleConnection(Connection.ConnectionString());
                var sql = $"select * from LOG";
                var logEvent = await con.QueryAsync<LogEventDTO>(sql);
                return (List<LogEventDTO>)logEvent;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<LogEventDTO>> GetLogEventByDate(string fromDate, string toDate)
        {
            try
            {
                using var con = new OracleConnection(Connection.ConnectionString());
                var sql = @"Select * From LOG where " + " \"TimeStamp\"" + @" between :FromDate And :ToDate";
                var logEvent = await con.QueryAsync<LogEventDTO>(sql, new { FromDate = fromDate.ToUpper(), ToDate = toDate.ToUpper() });
                return (List<LogEventDTO>)logEvent;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdminSystem.Job.Interface
{
    public interface IDapperService
    {
        Task<T> QueryFirstOrDefaultAsyncWithDapper<T>(string sql, object parameters, string connectionString = null);
        Task<List<T>> QueryAsyncWithDapper<T>(string sql, object parameters, string connectionString = null);
        Task<int> ExecuteAsyncWithDapper(string sql, object parameters, string connectionString = null);
        Task<int> ExecuteAsyncWithDapperAndTransaction(string sql, object parameters, IDbConnection conn, IDbTransaction transaction = null);
        Task<int> ExecuteScalarAsyncWithDapper(string sql, object parameters, IDbConnection conn, IDbTransaction transaction = null);
    }
}

using System.Data;

namespace MinistryData.WorkerService.Misc.Interface
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

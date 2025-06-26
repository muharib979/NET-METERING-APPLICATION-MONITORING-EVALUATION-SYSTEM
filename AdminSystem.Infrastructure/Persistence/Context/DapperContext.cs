using Npgsql;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace CFEMS.Infrastructure.Persistence.Context;

public class DapperContext
{
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration) => _configuration = configuration;

    //public IDbConnection GetDbConnection() => new NpgsqlConnection(_configuration["DatabaseSettings:ConnectionString"]);
   // public IDbConnection GetDbConnection() => new SqlConnection(_configuration["DatabaseSettings:ConnectionString"]);
    public IDbConnection GetDbConnection() => new OracleConnection(_configuration["DatabaseSettings:ConnectionString"]);


}

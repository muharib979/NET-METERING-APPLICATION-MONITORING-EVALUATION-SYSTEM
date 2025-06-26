using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Org.BouncyCastle.Crypto;

namespace CFEMS.Infrastructure.Persistence.Repositories.Dbo;

public class TokenRepository : ITokenRepository
{
    private readonly IDbConnection _db;

    public TokenRepository(DapperContext context) => _db = context.GetDbConnection();

    public async Task<int> AddAsync(Token entity)
    {
        entity.LAST_MODIFIED_DATE = DateTime.Now;
        var sql = "INSERT INTO MISCBILLAPP_TOKEN (value, created_date, user_id, last_modified_date, expiry_time, session_id) VALUES(:value, " + $" TO_DATE( '" + $"{entity.CREATED_DATE.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @" :user_id, " + $" TO_DATE( '" + $"{entity.LAST_MODIFIED_DATE.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @$" TO_DATE( '" + $"{entity.EXPIRY_TIME.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @" :session_id) ";
        return await _db.ExecuteAsync(sql, new { value = entity.VALUE, created_date = entity.CREATED_DATE, user_id = entity.USER_ID, last_modified_date = entity.LAST_MODIFIED_DATE, expiry_time = entity.EXPIRY_TIME, session_id = entity.SESSION_ID});
    }

    public Task<int> AddListAsync(List<Token> entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public async Task<int> DeleteByUserIdAsync(int id)
    {
        var sql = @"delete from MISCBILLAPP_TOKEN where user_id =:id";

        var deletedId = await _db.ExecuteAsync(sql, new {id = id });

        if (deletedId > 0)
        {
            return deletedId;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> DeleteExpireRefreshToken()
    {
        var sql = @"delete from MISCBILLAPP_TOKEN where TRUNC(expiry_time) < TRUNC(TO_DATE( '" + $"{DateTime.Now}" + "',  'YYYY-MM-DD HH24:MI:SS'))";
        return await _db.ExecuteAsync(sql);
    }

    public Task<List<Token>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

    public Task<Token> GetByIdAsync(int id) => throw new NotImplementedException();

    public async Task<Token> GetByRefreshtokenAsync(string refreshToken)
    {
        var result = await _db.QueryAsync<Token>("select * from MISCBILLAPP_TOKEN where  value= :refreshToken", new { refreshToken });
        return result.FirstOrDefault();
    }

    public async Task<Token> GetBySessionIdAsync(string sessionId)
    {
        var result = await _db.QueryAsync<Token>("select * from MISCBILLAPP_TOKEN where session_id= :sessionId", new { sessionId });
        return result.FirstOrDefault();
    }

    public Task<int> GetTotalCountAsync(string searchBy) => throw new NotImplementedException();

    public Task<int> UpdateAsync(Token entity) => throw new NotImplementedException();
}

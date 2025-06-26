using Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.ProsoftDataSyncRepositories
{
    public class ProsoftTokenRepository : IProsoftTokenRepository
    {
        private readonly IDbConnection _db;

        public ProsoftTokenRepository(DapperContext context) => _db = context.GetDbConnection();

        public async Task<int> AddAsync(Token entity)
        {
            entity.LAST_MODIFIED_DATE = DateTime.Now;
            var sql = "INSERT INTO MISCBILLAPP.TOKEN (value, created_date, user_id, last_modified_date, expiry_time, session_id) VALUES(:value, :created_date, :user_id, :last_modified_date, :expiry_time, :session_id)";
            return await _db.ExecuteAsync(sql, new { value = entity.VALUE, created_date = entity.CREATED_DATE, user_id = entity.USER_ID, last_modified_date = entity.LAST_MODIFIED_DATE, expiry_time = entity.EXPIRY_TIME, session_id = entity.SESSION_ID });
        }

        public Task<int> AddListAsync(List<Token> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

        public async Task<int> DeleteByUserIdAsync(string id)
        {
            var sql = @"delete from MISCBILLAPP.TOKEN where col_user_id =:id";

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
            var sql = @"delete from MISCBILLAPP.TOKEN where col_expiry_time < @dateTimeNow";
            return await _db.ExecuteAsync(sql, new { dateTimeNow = DateTime.Now });
        }

        public Task<List<Token>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

        public Task<Token> GetByIdAsync(int id) => throw new NotImplementedException();

        public async Task<Token> GetByRefreshtokenAsync(string refreshToken)
        {
            var result = await _db.QueryAsync<Token>("select * from MISCBILLAPP.TOKEN where  value= @refreshToken", new { refreshToken });
            return result.FirstOrDefault();
        }

        public async Task<Token> GetBySessionIdAsync(string sessionId)
        {
            var result = await _db.QueryAsync<Token>("select * from MISCBILLAPP.TOKEN where session_id= :sessionId", new { sessionId });
            return result.FirstOrDefault();
        }

        public Task<int> GetTotalCountAsync(string searchBy) => throw new NotImplementedException();

        public Task<int> UpdateAsync(Token entity) => throw new NotImplementedException();
    }
}

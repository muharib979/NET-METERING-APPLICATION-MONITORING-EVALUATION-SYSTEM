using Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface;
using Core.Domain.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.ProsoftDataSyncRepositories
{
    public class ProsoftUserRepository : IProsoftUserRepository
    {
        protected readonly IDbConnection db;
        public ProsoftUserRepository(DapperContext context)
        {
            this.db = context.GetDbConnection();
        }
        public async Task<int> AddProsoftUser(ProsoftUsers users)
        {
            var sql = @"INSERT INTO MISCBILLAPP.PROSOFT_USERS (user_name, full_name, email, password, password_salt, token, token_createdate, token_modifydate, token_expirydate, is_active, entry_by, entry_date, updated_by, updated_date)
              VALUES(:USER_NAME, :FULL_NAME, :EMAIL, :PASSWORD, :PASSWORD_SALT, :TOKEN, :TOKEN_CREATEDATE, :TOKEN_MODIFYDATE, :TOKEN_EXPIRYDATE,:IS_ACTIVE, :ENTRY_BY, :ENTRY_DATE, :UPDATED_BY, :UPDATED_DATE)";

            return await db.ExecuteAsync(sql,users);
        }

        public async Task<ProsoftUsers> GetUserByName(string name)
        {
            var sql = @"select * from PROSOFT_USERS a where a.col_user_name=:name";
            return await db.QueryFirstOrDefaultAsync<ProsoftUsers>(sql, new {name = name });
        }


        public async Task<int> UpdateUserTokenInfo(ProsoftUsers users)
        {
            var sql = @"update MISCBILLAPP.PROSOFT_USERS 
                            set  token = @col_token,
                            token_createdate = :col_token_createdate,
                            token_modifydate = :col_token_modifydate,
                            token_expirydate = :col_token_expirydate
                        where col_user_id = :col_user_id";
            return await db.ExecuteAsync(sql, new { col_token = users.TOKEN, col_token_createdate = users.TOKEN_CREATEDATE, col_token_expirydate = users.TOKEN_EXPIRYDATE, col_token_modifydate = users.TOKEN_MODIFYDATE, col_user_id = users.ID });
        }


        public async Task<ProsoftUsers> GetByRefreshtokenAsync(string refreshToken)
        {
            var sql = @"Select * from MISCBILLAPP.PROSOFT_USERS a where a.token=:col_token and a.is_active=1";
            return await db.QueryFirstOrDefaultAsync<ProsoftUsers>(sql, new { col_token = refreshToken });
        }

        public async Task<ProsoftUsers> GetUserByIdAsync(int userid)
        {
            var sql = @"select * from PROSOFT_USERS a where a.user_id = :userid";
            return await db.QueryFirstOrDefaultAsync<ProsoftUsers>(sql, new { userid = userid });
        }


        public Task<int> AddAsync(ProsoftUsers entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<ProsoftUsers> entity)
        {
            throw new NotImplementedException();
        }        

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProsoftUsers>> GetAllAsync(PaginationParams pParams)
        {
            throw new NotImplementedException();
        }

        public Task<ProsoftUsers> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(ProsoftUsers entity)
        {
            throw new NotImplementedException();
        }

    }
}

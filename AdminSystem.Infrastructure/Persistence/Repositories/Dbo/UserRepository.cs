using AutoMapper;
using Core.Application.Commands.Dbo.UserAdd;
using Core.Application.Commands.Dbo.UserEdit;
using Core.Domain.Location;
using Microsoft.AspNetCore.Http;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Enums;

namespace CFEMS.Infrastructure.Persistence.Repositories.Dbo;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _db;
    private readonly IMapper _mapper;

    public UserRepository(DapperContext context, IMapper mapper)
    {
        _db = context.GetDbConnection();
        _mapper = mapper;
    }



    public Task<int> AddAsync(User entity) => throw new NotImplementedException();

    public async Task<int> AddUserAsync(User entity, int roleId)
    {
        int status = 0;
        if (entity == null) return await Task.FromResult(status);


        using (IDbConnection conn = _db)
        {
            conn.Open();
            using var transection = _db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            var sql = @"insert into  MISCBILLAPP_USER_DETAILS (user_name, password, password_salt, email, full_name, is_active, entry_by, entry_date, is_deleted )
                    values (:user_name, :password, :password_salt, :email, :full_name, :is_active, :entry_by, " + $" TO_DATE( '" + $"{entity.ENTRY_DATE.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @" :is_deleted)";
            var i = await _db.ExecuteAsync(
                                sql,
                                new
                                {
                                    user_name = entity.USER_NAME,
                                    password = entity.PASSWORD,
                                    password_salt = entity.PASSWORD_SALT,
                                    email = entity.EMAIL,
                                    full_name = entity.FULL_NAME,
                                    is_active = entity.IS_ACTIVE,
                                    entry_by = entity.ENTRY_BY,
                                    entry_date = entity.ENTRY_DATE.ToString("yyyy-M-dd hh: mm:ss"),
                                    is_deleted = entity.IS_DELETED
                                }, transection);
            var sQuery = @"select MISCBILLAPP_USER_DETAILS_SQ.currval from dual";
            var userId = await _db.ExecuteScalarAsync<int>(sQuery, null, transection);

            var insert = _db.Execute("INSERT INTO  MISCBILLAPP_USER_TO_ROLE(user_id_fk, role_id_fk, is_active) VALUES (:UserId, :RoleId, :IsActive)", new { UserId = userId, RoleId = roleId, IsActive = (int)BooleanEnum.TRUE }, transection);

            if (insert > 0)
            {
                transection.Commit();
                status = 1;
            }
            else
            {
                transection.Rollback();
                status = 0;
            }
            //transection.Complete();
            transection.Dispose();


            return await Task.FromResult(status);
        }


    }

    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public Task<List<User>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

    public async Task<List<UserDto>> GetAllUserAsync(PaginationParams pParams)
    {
        string sql = @"Select * from (SELECT
                        COUNT(*) OVER() as TotalRowCount,
                        ROW_NUMBER() OVER (ORDER BY u.Id) AS RowNumber,
                        u.id as UserId,
                        u.full_name as FullName,
                        u.user_name as UserName,
                        u.email as Email,
                        r.role_name as RoleName
                        FROM  MISCBILLAPP_USER_DETAILS u
                        left join  MISCBILLAPP_USER_TO_ROLE ur on  ur.user_id_fk = u.id
						left join  MISCBILLAPP_MS_ROLE r on  r.id = ur.role_id_fk";

        if (pParams.SearchBy != null)
        {
            sql += @" WHERE u.user_name  like   '%' + :searchBy + '%' AND u.is_deleted = 0 ) temp"; // 0 is false
        }
        else
        {
            sql += @" WHERE u.is_deleted = 0 ) temp"; // 0 is false
        }


        var users = await _db.QueryAsync<UserDto>(sql, new { startRow = ((pParams.PageNumber - 1) * pParams.pageSize) + 1, endRow = (pParams.pageSize * pParams.PageNumber) });


        return users.ToList();
    }

    public async Task<User> GetByNameAsync(string name)
    {


        var sql = "select * from  MISCBILLAPP_USER_DETAILS  where user_name = :name and is_deleted = :isFalse";
        var user = await _db.QueryAsync<User>(sql, new { name, isFalse = (int)BooleanEnum.FALSE });
        return user.FirstOrDefault();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var sql = "select * from  MISCBILLAPP_USER_DETAILS  where id = :id and is_deleted = :isFalse ";
        var user = await _db.QueryAsync<User>(sql, new { id, isFalse = (int)BooleanEnum.FALSE });
        return user.FirstOrDefault();
    }

    public Task<int> UpdateAsync(User entity) => throw new NotImplementedException();

    public async Task<int> GetTotalCountAsync(string searchBy)
    {
        string countSql = @"SELECT COUNT (DISTINCT id)
                            FROM  MISCBILLAPP_USER_DETAILS u";
        if (searchBy != null)
        {
            countSql += @" WHERE u.user_name  like   '%' + @searchBy + '%'";
        }

        var totalCount = await _db.QueryAsync<int>(countSql, new { searchBy });
        return totalCount.FirstOrDefault();
    }

    public async Task<List<DropdownResult>> GetAllUserDDAsync()
    {
        var sql = $"SELECT id as Key, user_name as Value from  MISCBILLAPP_USER_DETAILS WHERE is_deleted = :isFalse";
        var dDres = await _db.QueryAsync<DropdownResult>(sql, new { isFalse = (int)BooleanEnum.FALSE });
        return dDres.ToList();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var sql = "select * from  MISCBILLAPP_USER_DETAILS  where email = :email";
        var user = await _db.QueryAsync<User>(sql, new { email });
        return user.FirstOrDefault();
    }

    public async Task<int> UpdateUserOtp(User entity)
    {

        var sql = @"update  MISCBILLAPP_USER_DETAILS set otp = :otp, otp_expiry_time = :otp_expiry_time where id  = :user_id";
        var updatedId = await _db.ExecuteAsync(sql, new { user_id = entity.ID, otp = entity.OTP, otp_expiry_time = entity.OTP_EXPIRY_TIMIE });

        if (updatedId > 0)
        {
            return updatedId;
        }
        else
        {
            return 0;
        }
    }

    public async Task<User> VerifyOtp(int code, string email)
    {
        var sql = "select * from  MISCBILLAPP_USER_DETAILS  where col_otp = :otp and email = :email";
        var user = await _db.QueryFirstOrDefaultAsync<User>(sql, new { otp = code, email });

        return user;
    }

    public async Task<int> ChangePassword(User entity)
    {
        int status = 0;
        if (entity == null) return status;

        var sql = @"update  MISCBILLAPP_USER_DETAILS set password = :password_hash, password_salt = :password_salt where USER_NAME  = :userName";
        var userId = await _db.ExecuteAsync(sql, new { password_hash = entity.PASSWORD, password_salt = entity.PASSWORD_SALT, userName = entity.USER_NAME });

        status = 1;

        return status;
    }

    public async Task<Role> GetRolesByUserId(int userId)
    {
        var sql = @"select r.id,r.role_name,r.is_active,r.menu_id_fk from  MISCBILLAPP_MS_ROLE r
                        join  MISCBILLAPP_USER_TO_ROLE utr on utr.role_id_fk = r.id
                        where utr.user_id_fk =:userId and r.is_active = :isTrue ";
        return await _db.QueryFirstOrDefaultAsync<Role>(sql, new { userId, isTrue = (int)BooleanEnum.TRUE });
    }

    public async Task<List<string>> GetDbCodeByUserId(int userId)
    {
        var sql = @"SELECT DB_CODE FROM  MISCBILLAPP_MS_DB_MAPPING WHERE USERID=:userId ";
        return (List<string>)await _db.QueryAsync<string>(sql, new { userId });
    }

    public async Task<List<string>> GetLocationCode(int userId)
    {
        var sql = @"SELECT l.CODE from MISCBILLAPP_MS_DB_MAPPING m 
                INNER JOIN MISCBILLAPP_MS_LOC_MAPPING lm ON m.ID=lm.DM_ID
                INNER JOIN MISCBILLAPP_MS_LOCATION l ON l.ID=lm.LOCATION_ID
                WHERE m.USERID=:userId ";
        return (List<string>)await _db.QueryAsync<string>(sql, new { userId });
    }

    public async Task<int> EditUserAsync(UserEditCommand model)
    {
        int status = 0;

        using var transection = new TransactionScope();

        var timestamp = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
        var sql = @"update  MISCBILLAPP_USER_DETAILS set full_name = :full_name, user_name =:user_name, email= :email, updated_by = :updated_by, updated_date = " + $" TO_DATE( '" + $"{timestamp}" + "',  'YYYY-MM-DD HH24:MI:SS')" + @" where id = :user_id";

        _db.ExecuteScalar<int>(sql, new { full_name = model.FirstName + " " + model.LastName, user_name = model.UserName, email = model.Email, updated_by = model.UpdatedBy, updated_date = timestamp, user_id = model.UserId });

        var userToRole = _db.QuerySingleOrDefault<UserToRole>("select * from  MISCBILLAPP_USER_TO_ROLE where user_id_fk =:UserId", new { UserId = model.UserId });

        if (userToRole != null)
        {
            _db.Execute("update  MISCBILLAPP_USER_TO_ROLE set role_id_fk = :RoleId where user_id_fk =:UserId", new { UserId = model.UserId, RoleId = model.RoleId });
        }
        else
        {
            _db.Execute("INSERT INTO  MISCBILLAPP_USER_TO_ROLE(user_id_fk, role_id_fk, is_active) VALUES (:UserId, :RoleId, :IsActive)", new { UserId = model.UserId, RoleId = model.RoleId, IsActive = (int)BooleanEnum.TRUE });
        }

        transection.Complete();
        transection.Dispose();
        status = 1;

        return await Task.FromResult(status);
    }

    public Task<int> AddListAsync(List<User> entity)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddUserByCenterLocationAsync(UserCreateByCenterLocation entity, int roleId)
    {
        int status = 0;
        if (entity == null) return await Task.FromResult(status);

        using IDbConnection conn = _db;
        conn.Open();
        using var transection = _db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

        var sql = @"insert into  MISCBILLAPP_USER_DETAILS (USER_NAME,PASSWORD,PASSWORD_SALT, ROLE_ID,ENTRY_BY,ENTRY_DATE, IS_DELETED)
                    values (:UserId,:UserPassword, :PasswordSalt, :RoleId,:CreatedBy, " + $" TO_DATE( '" + $"{entity.ENTRY_DATE.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @" :is_deleted)";

        var i = await _db.ExecuteAsync(
                                sql,
                                new
                                {
                                    UserId = entity.USER_NAME,
                                    UserPassword = entity.PASSWORD,
                                    PasswordSalt = entity.PASSWORD_SALT,
                                    RoleId = roleId,

                                    CreatedBy = entity.ENTRY_BY,
                                    CreatedDate = entity.ENTRY_DATE.ToString("yyyy-M-dd hh: mm:ss"),
                                    is_deleted = entity.IS_DELETED
                                }, transection);
        var sQuery = @"select  MISCBILLAPP_USER_DETAILS_SQ.currval from dual";
        var userId = await _db.ExecuteScalarAsync<int>(sQuery, null, transection);

        var insert = _db.Execute("INSERT INTO  MISCBILLAPP_USER_TO_ROLE(user_id_fk, role_id_fk, is_active) VALUES (:UserId, :RoleId, :IsActive)", new { UserId = userId, RoleId = roleId, IsActive = (int)BooleanEnum.TRUE }, transection);

        if (insert > 0 && await DbLocationMappingConfig(entity.DB, entity.Location, userId, transection))
        {
            transection.Commit();
            status = 1;
        }
        else
        {
            transection.Rollback();
            status = 0;
        }
        transection.Dispose();


        return await Task.FromResult(status);

    }

    /// <summary>
    /// Gets database and location mapping insert status.
    /// </summary>
    /// <param name="dbs">List<int> dbs is the list of new user's databases</param>
    /// <param name="locations">List<int> locations is the list of new user's locations</param>
    /// <param name="userId">int userId is the user id of new user</param>
    /// <returns>Boolean of success or failure of insert of database and location mapping</returns>
    public async Task<bool> DbLocationMappingConfig(List<int> dbs, List<int> locations, int userId, IDbTransaction tran)
    {
        int fullAccess = 0;
        if (dbs.Count >= 1 && (locations == null || locations.Count == 0))
        {
            fullAccess = 1;
        }

        var id = 0;
        try
        {

            if (dbs.Count > 0 && fullAccess == 1)
                id = await InsertDatabaseMapping(dbs, fullAccess, userId, tran);

            if (locations != null && locations.Count > 0 && dbs.Count == 1)
            {
                id = await InsertDatabaseMapping(dbs, fullAccess, userId, tran);
                id = await InsertLocationMapping(locations, id);
            }

            if (dbs.Count > 1 && locations != null && locations.Count > 0)
            {
                foreach (var item in dbs)
                {
                    var dm_Id = await InsertMulDatabaseMapping(item, fullAccess, userId, locations, tran);
                    if (dm_Id > 0)
                    {
                        var res = await InsertMultipleLocationMapping(locations, dm_Id, item);
                    }

                }

            }
        }
        catch (Exception)
        {
            tran.Rollback();
            tran.Dispose();
            throw;
        }

        return true;
    }

    private async Task<int> InsertDatabaseMapping(List<int> DB, int FullAccess, int userId, IDbTransaction tran)
    {
        int result = 0;

        try
        {
            foreach (var item in DB)
            {
                var iQuery = @"INSERT INTO  MISCBILLAPP_MS_DB_MAPPING(USERID,DB_CODE,FULLACCESS) VALUES(:userId,:item,:FullAccess)";
                await _db.ExecuteScalarAsync(iQuery, new { userId = userId, item = item, FullAccess = FullAccess }, tran);

                iQuery = @"select  MISCBILLAPP_MS_DB_MAPPING_SEQ.currval from dual";
                result = await _db.ExecuteScalarAsync<int>(iQuery, null, tran);
            }

        }
        catch (Exception)
        {
            tran.Rollback();
            tran.Dispose();
            throw;
        }


        return result;

    }

    private async Task<int> InsertLocationMapping(List<int> Location, int dbId)
    {
        int result = 0;
        try
        {
            foreach (var item in Location)
            {
                var iQuery = "INSERT INTO  MISCBILLAPP_MS_LOC_MAPPING(DM_ID,LOCATION_ID) VALUES(:dbId,:LocationId)";
                result = await _db.ExecuteScalarAsync<int>(iQuery, new { dbId = dbId, LocationId = item });
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }

    private async Task<int> InsertMulDatabaseMapping(int DB, int FullAccess, int userId, List<int> Location, IDbTransaction tran)
    {
        int result = 0;

        try
        {

            var iQuery = @"INSERT INTO  MISCBILLAPP_MS_DB_MAPPING(USERID,DB_CODE,FULLACCESS) VALUES(:userId,:item,:FullAccess)";
            await _db.ExecuteScalarAsync(iQuery, new { userId = userId, item = DB, FullAccess = FullAccess }, tran);

            iQuery = @"select  MISCBILLAPP_MS_DB_MAPPING_SEQ.currval from dual";
            result = await _db.ExecuteScalarAsync<int>(iQuery, null, tran);

        }
        catch (Exception)
        {
            tran.Rollback();
            tran.Dispose();
            throw;
        }
        return result;

    }
    private async Task<int> InsertMultipleLocationMapping(List<int> Location, int DM_id, int dbId)
    {
        int result = 0;
        try
        {

            string locQuery = " ";
            foreach (var loc in Location)
            {
                locQuery += loc.ToString() + ", ";

            }
            locQuery = locQuery.TrimEnd(',', ' ');
            var sQuery = $"SELECT  MISCBILLAPP_MS_LOCATION.Id FROM MISCBILLAPP_MS_LOCATION WHERE MISCBILLAPP_MS_LOCATION.DB_CODE=: db AND MISCBILLAPP_MS_LOCATION.id in ({locQuery})";
            var response = await _db.QueryAsync<Locations>(sQuery, new { db = dbId }); //QueryAsyncWithDapper

            foreach (var responeLocation in response)
            {
                var iQ = "INSERT INTO  MISCBILLAPP_MS_LOC_MAPPING(DM_ID,LOCATION_ID) VALUES(:dbId,:LocationId)";
                result = await _db.ExecuteScalarAsync<int>(iQ, new { dbId = DM_id, LocationId = responeLocation.ID });
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }

    public async Task<List<UserRole>> GetUsersRolesAsync()
    {
        var sql = $"select  MISCBILLAPP_USER_DETAILS.id  Key,  MISCBILLAPP_USER_DETAILS.user_name  Value, r.ROLE_NAME  roleName  from  MISCBILLAPP_USER_DETAILS INNER JOIN  MISCBILLAPP_MS_ROLE r on  MISCBILLAPP_USER_DETAILS.role_id = r.ID where is_deleted = :isFalse";
        var usersRoles = await _db.QueryAsync<UserRole>(sql, new { isFalse = (int)BooleanEnum.FALSE });
        return usersRoles.ToList();
    }

    public Task AddUserAsync(UserAddCommand request)
    {
        throw new NotImplementedException();
    }

    public async Task<int> DeleteNewUserByCenterLocation(int id)
    {
        using var con = new OracleConnection(AdminSystem.Infrastructure.Persistence.Context.Connection.ConnectionString());
        var sql = $@"UPDATE  MISCBILLAPP_USER_DETAILS U SET U.IS_DELETED = 1 WHERE U.id = :UserId";
        var deletedId = await con.ExecuteAsync(sql, new { UserId = id });

        if (deletedId > 0)
        {
            return deletedId;
        }
        else
        {
            return 0;
        }
    }





    public async Task<List<GetUserCreateByCenterLocationModel>> GetNewUserByCenterLocationById(int userId)
    {
        var sql = @" SELECT   U.ROLE_ID ROLE_ID, DM.DB_CODE, DM.FULLACCESS, DM.USERID, LM.LOCATION_ID, U.USER_NAME USER_NAME FROM
                            MISCBILLAPP_MS_DB_MAPPING DM
                            INNER JOIN MISCBILLAPP_USER_DETAILS  U ON U.ID = DM.USERID
                            LEFT JOIN MISCBILLAPP_MS_LOC_MAPPING LM on LM.DM_ID = DM.ID
                            WHERE U.id = :id and U.is_deleted=:isFalse";

        var user = await _db.QueryAsync<GetUserCreateByCenterLocationModel>(sql, new { id = userId, isFalse = (int)BooleanEnum.FALSE });
        return user.ToList();

    }
    /// <summary>
    /// Deletes database mappings of user.
    /// </summary>
    /// <param name="user">string userId is the user id of new user</param>
    /// <param name="dbs">List<int> dbs is the list of new user's databases</param>
    /// <param name="locations">List<int> locations is the list of new user's locations</param>
    /// <returns>Boolean of success or failure of delete of database mapping</returns>
    public async Task<bool> UpdateNewUserDatabaseAndLocationMapping(int UserId, UserCreateByCenterLocation user, List<int> dbs, List<int> locations)
    {
        try
        {

            using (IDbConnection conn = _db)
            {
                conn.Open();
                using var transection = _db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    if (await DeleteDatabaseMapping(UserId)
                        && await UpdateUserToAdminAsync(UserId, user) >= 1 && await DbLocationMappingConfig(dbs, locations, UserId, transection)
                        )
                    {
                        transection.Commit();
                        return true;
                    }
                    else
                    {
                        transection.Rollback();
                        return false;
                    }
                }
                catch (Exception)
                {
                    transection.Rollback();
                    throw;
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> DeleteDatabaseMapping(int userId)
    {
        try
        {
            using var con = new OracleConnection(AdminSystem.Infrastructure.Persistence.Context.Connection.ConnectionString());
            var sql = "DELETE FROM MISCBILLAPP_MS_DB_MAPPING WHERE USERID = :userId";
            if (await con.ExecuteAsync(sql, new { userId }) >= 1)
                return true;
        }
        catch (Exception)
        {
            throw;
        }
        return false;
    }
    public async Task<int> UpdateUserToAdminAsync(int id, UserCreateByCenterLocation user)
    {
        int inserted;
        using var con = new OracleConnection(AdminSystem.Infrastructure.Persistence.Context.Connection.ConnectionString());
        try
        {
            var iQuery = "UPDATE MISCBILLAPP_USER_DETAILS SET " +
                "ROLE_ID= :RoleId," +
                "UPDATED_BY =:UpdateBy," +
                "UPDATED_DATE = " + $" TO_DATE( '" + $"{DateTime.Now.ToString("yyyy-M-dd hh: mm:ss")}" + "',  'YYYY-MM-DD HH24:MI:SS') " +
                " WHERE ID= :Id";

            inserted = await con.ExecuteAsync(iQuery, new
            {
                Id = id,
                RoleId = user.ROLE_ID,

                UpdateBy = user.UPDATED_BY,

            });
        }
        catch (Exception ex)
        {
            throw;
        }
        return inserted;
    }

    public async Task<List<DropdownResult>> GetAllUserDdByPriorityAndUser(string userName, string locationCode)
    {
        var sql = @"select u.id as Key, u.USER_NAME as Value from MISCBILLAPP_USER_DETAILS U
                    inner join MISCBILLAPP_MS_ROLE R ON R.ID=U.ROLE_ID
                    inner join MISCBILLAPP_MS_DB_MAPPING D on D.USERID=U.ID
                    INNER JOIN MISCBILLAPP_MS_LOC_MAPPING LM ON lm.dm_id=d.id
                    INNER JOIN MISCBILLAPP_MS_LOCATION L ON l.id=lm.location_id
                    where u.is_deleted = :isFalse and r.priority >= (select DISTINCT R.PRIORITY from MISCBILLAPP_MS_ROLE R 
                            INNER JOIN MISCBILLAPP_USER_DETAILS U ON R.ID = U.ROLE_ID WHERE UPPER(u.USER_NAME)=UPPER(:userName))
                            AND l.code=:locationCode
                            and UPPER(u.USER_NAME)<>UPPER(:userName) 
                            ORDER BY PRIORITY ASC";
        var result = await _db.QueryAsync<DropdownResult>(sql, new { userName = userName.ToUpper(), locationCode = locationCode, isFalse = (int)BooleanEnum.FALSE });
        return result.ToList();
    }
}

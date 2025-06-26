using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Commands.Dbo.RoleAdd;
using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Core.Domain.Dbo;
using Core.Domain.ZoneCircle;
using FakeItEasy;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Enums;

namespace CFEMS.Infrastructure.Persistence.Repositories.Dbo;

public class RoleRepository : IRoleRepository
{
    private IMapper _mapper;
    private readonly IMenuRepository _menuRepository;

    public RoleRepository(IMapper mapper, IMenuRepository menuRepository)
    {
        _mapper = mapper;
        _menuRepository = menuRepository;
    }


    public async Task<int> AddAsync(Role entity)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var status = 0;
        
        using (IDbConnection conn = con)
        {
            conn.Open();
            using var transection = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            var sql = @"insert into MISCBILLAPP_MS_ROLE ( role_name, is_active,priority) values (:ROLE_NAME, :IS_ACTIVE,:PRIORITY)";

           var insert =await con.ExecuteAsync(sql, new
            {
                role_name = entity.ROLE_NAME,
                is_active = entity.IS_ACTIVE,
                priority=entity.PRIORITY
             }, transection);

            
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
            transection.Dispose();
            return status;
        }
    }

    public async Task<int> UpdateAsync(Role entity)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var status = 0;
        var timeStamp = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
        using var transection = new TransactionScope();

        var role = con.QueryFirstOrDefault<Role>("select * from MISCBILLAPP_MS_ROLE where id =:RoleId", new { RoleId = entity.ID });

        var sql = @"update MISCBILLAPP_MS_ROLE set role_name=:role_name, is_active=:is_active,priority=:priority where id= :role_id";
        //var sql = @"update MISCBILLAPP_MS_ROLE set role_name=:role_name, menu_id_fk=:menu_fk_id, is_active=:is_active where id= :role_id"; //old code

        con.ExecuteScalar<int>(sql, new
        {
            role_id = entity.ID,
            role_name = entity.ROLE_NAME,
             
            is_active = entity.IS_ACTIVE,
            Priority= entity.PRIORITY,
        });
        

        transection.Complete();
        transection.Dispose();

        status = 1;

        return status;
    }

    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public async Task<List<RoleDto>> GetAllWithMenuNameAsync(PaginationParams pParams)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        string sql = @"Select * from (SELECT
                       COUNT(*) OVER() as TotalRowCount, 
                       ROW_NUMBER() OVER (ORDER BY r.Id) AS RowNumber,
                       r.id as Id,
                       r.role_name as RoleName,
                       r.is_sms_role as IsSmsRole,
                       r.menu_id_fk as MenuFkId,
                       m.menu_name as MenuName,
                       r.is_active as IsActive,
                       r.PRIORITY AS Priority 
                        FROM MISCBILLAPP_MS_ROLE  r
                        left join MISCBILLAPP_MENU m on r.menu_id_fk = m.id";

        if (pParams.SearchBy != null)
        {
            sql += @" WHERE r.role_name  like   '%' + @searchBy + '%' And r.is_active = 1 ) temp"; // 1 is true
        }
        else
        {
            sql += @" WHERE r.is_active = 1 ) temp"; // 1 is true
        }

        var roles = await con.QueryAsync<RoleDto>(sql, new {  startRow = ((pParams.PageNumber ) * pParams.pageSize) + 1, endRow = (pParams.pageSize * pParams.PageNumber), searchBy = pParams.SearchBy } );

        return roles.ToList();
    }

    public async Task<List<DropdownResult>> GetAllRoleDDAsync()
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = $"SELECT id  Key, role_name  Value,PRIORITY Priority from MISCBILLAPP_MS_ROLE where is_active =: isTrue"; //and col_is_sms_roles =false
        var dDres = await con.QueryAsync<DropdownResult>(sql, new {isTrue = (int)BooleanEnum.TRUE});
        return dDres.ToList();
    }

    public async Task<Role> GetByIdAsync(int id)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"SELECT r.id, r.role_name, r.menu_id_fk, r.is_active,r.priority
                        FROM MISCBILLAPP_MS_ROLE r
                        where r.id = :id and is_active =:isTrue";

        var role = await con.QueryAsync<Role>(sql, new { id, isTrue = (int)BooleanEnum.TRUE });
        return role.FirstOrDefault();
    }

    public async Task<int> GetTotalCountAsync(string searchBy)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        string countSql = @"SELECT COUNT (DISTINCT id)
                            FROM MISCBILLAPP_MS_ROLE r";
        if (searchBy != null)
        {
            countSql += @" WHERE r.role_name  like   '%' + @searchBy + '%'";
        }

        var totalCount = await con.QueryAsync<int>(countSql, new { searchBy });
        return totalCount.FirstOrDefault();
    }


    public Task<List<Role>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

    public Task<int> AddListAsync(List<Role> entity)
    {
        throw new NotImplementedException();
    }

    public async Task<int> DeleteRoleAsync(int id)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"DELETE FROM MISCBILLAPP_MS_ROLE R WHERE R.ID = :roleId";
        var removeRoleId = await con.ExecuteAsync(sql, new { roleId = id });
        if (removeRoleId > 0)
        {
            return removeRoleId;
        }
        else
        {
            return 0;
        }
    }

    public async Task<List<RoleDto>> GetRoleByUserName(string userName)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        List<RoleDto> dto = new List<RoleDto>();
        string query = $@"SELECT * FROM MISCBILLAPP_MS_ROLE  WHERE PRIORITY>=( select DISTINCT R.PRIORITY from MISCBILLAPP_MS_ROLE R 
                            INNER JOIN MISCBILLAPP_USER_DETAILS U ON R.ID = U.ROLE_ID WHERE UPPER(u.USER_NAME)=:userName) ORDER BY PRIORITY ASC";
        var result = await con.QueryAsync<Role>(query, new { UserName = userName.ToUpper()});
        dto = _mapper.Map(result.ToList(), dto);
        return dto;
    }

    public async Task<List<RoleDto>> GetRoleForAccessMappingByUser(string userName)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        List<RoleDto> dto = new List<RoleDto>();
        string query = $@"SELECT * FROM MISCBILLAPP_MS_ROLE  WHERE PRIORITY>=( select DISTINCT R.PRIORITY from MISCBILLAPP_MS_ROLE R 
                            INNER JOIN MISCBILLAPP_USER_DETAILS U ON R.ID = U.ROLE_ID WHERE UPPER(u.USER_NAME)=:userName) ORDER BY PRIORITY ASC";
        var result = await con.QueryAsync<Role>(query, new { UserName = userName.ToUpper() });
        dto = _mapper.Map(result.ToList(), dto);
        return dto;
    }
}

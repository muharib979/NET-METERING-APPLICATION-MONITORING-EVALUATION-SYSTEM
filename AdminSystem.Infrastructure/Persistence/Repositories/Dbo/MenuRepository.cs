using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Enums;

namespace CFEMS.Infrastructure.Persistence.Repositories.Dbo;

public class MenuRepository : IMenuRepository
{
    
    private IMapper _mapper;

    public MenuRepository(IMapper mapper) 
    {
        _mapper = mapper;
    }

    public async Task<int> AddAsync(Menu entity)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var timestamp = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
        var sql = @"insert into MISCBILLAPP_MENU (menu_name,url, is_active, timestamp, icon, parent_id, is_parent, is_group, group_id,order_no) values (:MENU_NAME, :URL,:IS_ACTIVE, " + $" TO_DATE( '" + $"{timestamp}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @" :ICON, :PARENT_ID, :IS_PARENT, :IS_GROUP, :GROUP_ID,:ORDER_NO)";
        return await con.ExecuteAsync(sql, entity);
    }
    public async Task<int> UpdateAsync(Menu entity)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var timestamp = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
        var sql = @"update MISCBILLAPP_MENU set menu_name=:MENU_NAME, url=:URL, is_active=:IS_ACTIVE, TIMESTAMP=" + $" TO_DATE( '" + $"{timestamp}" + "',  'YYYY-MM-DD HH24:MI:SS')," + @" ICON=:ICON, parent_id=:PARENT_ID, is_parent=:IS_PARENT, is_group=:IS_GROUP,order_no=:ORDER_NO where id=:ID";
        return await con.ExecuteAsync(sql, entity);
    }

    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public async Task<List<Menu>> GetAllAsync(PaginationParams pParams)
    {
       
        using var con = new OracleConnection(Connection.ConnectionString());

       
        string sql = $"SELECT * FROM MISCBILLAPP_MENU";
        var result = con.Query<Menu>(sql).ToList();
        return result;
    }

    public async Task<List<Menu>> GetAllDashBoardMenuDDAsync()
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"SELECT *
                    from MISCBILLAPP_MENU
                    where is_group=:isfalse and id  not in (select COALESCE(parent_id,0) from MISCBILLAPP_MENU) and is_active =:isActive  order by id";
        var dDres = await con.QueryAsync<Menu>(sql, new { isfalse = (int)BooleanEnum.FALSE, isActive = (int)BooleanEnum.TRUE });
        return dDres.ToList();
    }

    
    
    public async Task<List<Menu>> GetAllDashBoardMenu(int roleId)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"select 	mn.id,
		                mn.menu_name,
		                mn.url,
		                mn.is_parent,
		                mn.parent_id,
		                mn.icon,
		                mn.is_active,
		                mn.is_dashboard,
		                mn.is_group,
		                mn.home,
		                mn.group_id
                from MISCBILLAPP_MENU mn
                join MISCBILLAPP_MS_ROLE r on r.menu_id_fk = mn.id 
                where r.id = :roleId and mn.home = :isActive 
                order by mn.id";

        var dDres = await con.QueryAsync<Menu>(sql, new { roleId, isActive = (int)BooleanEnum.TRUE });
        return dDres.ToList();
    }

    public async Task<List<Menu>> GetAllMenu()
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"SELECT *
                        FROM MISCBILLAPP_MENU mn 
                        where is_group = :isActive and is_active = :isActive
                        order by mn.id ";

        var menu = await con.QueryAsync<Menu>(sql, new { isActive = (int)BooleanEnum.TRUE });
        return menu.ToList();
    }

    public async Task<List<DropdownResult>> GetAllParentMenuDDAsync()
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = $"SELECT id  Key, menu_name  Value from MISCBILLAPP_MENU where is_parent =:isActive and is_active =:isActive";
        var dDres = await con.QueryAsync<DropdownResult>(sql, new { isActive = (int)BooleanEnum.TRUE } );
        return dDres.ToList();
    }

    public async Task<Menu> GetByIdAsync(int id)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
       
        var sql = $"SELECT * FROM MISCBILLAPP_MENU where id = {id} and is_active =1";

       
       var menu = await con.QueryAsync<Menu>(sql);
        return menu.FirstOrDefault();
    }

    public async Task<List<DropdownResult>> GetGroupDDAsync()
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = $"SELECT id  Key, menu_name  Value from MISCBILLAPP_MENU where is_group = :isActive and is_active = :isActive";
        var dDres = await con.QueryAsync<DropdownResult>(sql, new { isActive = (int)BooleanEnum.TRUE });
        return dDres.ToList();
    }

    public async Task<int> GetTotalCountAsync(string searchBy)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        string countSql = @"SELECT COUNT (DISTINCT id)
                            FROM MISCBILLAPP_MENU m";
        if (searchBy != null)
        {
            countSql += @"  WHERE m.menu_name  like   '%' + @searchBy + '%'";
        }

        var totalCount = await con.QueryAsync<int>(countSql, new { searchBy });
        return totalCount.FirstOrDefault();
    }


    public Task<int> AddListAsync(List<Menu> entity)
    {
        throw new NotImplementedException();
    }

    public Task<List<SVGICon>> GetSVGIconList()
    {
        throw new NotImplementedException();
    }

    public async Task<int> DeleteMenuById(int id)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = $@"DELETE FROM MISCBILLAPP_MENU M WHERE M.id = :Id";
        var deletedId = await con.ExecuteAsync(sql, new { Id = id });

        if (deletedId > 0)
        {
            return deletedId;
        }
        else
        {
            return 0;
        }
    }
    
}

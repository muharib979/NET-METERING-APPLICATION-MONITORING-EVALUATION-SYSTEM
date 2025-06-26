using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Commands.Dbo.AddRoleToMenu;
using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Enums;

namespace CFEMS.Infrastructure.Persistence.Repositories.Dbo;

public class RoleToMenuRepository : IRoleToMenuRepository
{
    
    private readonly IRoleRepository _roleRepository;
    private readonly IMenuRepository _menuRepository;

    public RoleToMenuRepository(DapperContext context, IRoleRepository roleRepository, IMenuRepository menuRepository)
    {
        
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
    }

    public async Task<int> AssignRoleToMenu(List<RoleToMenu> rtm)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        using var transaction = new TransactionScope();

        var role = _roleRepository.GetByIdAsync(rtm[0].ROLE_ID_FK).Result;
        var menu = _menuRepository.GetByIdAsync(role.MENU_ID_FK).Result;

        con.ExecuteScalar<int>("delete from MISCBILLAPP_ROLE_TO_MENU where role_id_fk = :Role_Fk_Id and menu_id_fk <> :Menu_Fk_Id", new { Role_Fk_Id = role.ID, Menu_Fk_Id = role.MENU_ID_FK });

        SaveRange(rtm);

        if (menu.PARENT_ID != null)
        {
            con.ExecuteScalar<int>(@"update MISCBILLAPP_ROLE_TO_MENU set is_active =:isTrue where role_id_fk = :Role_Fk_Id 
                                                and menu_id_fk =  :ParentId", new { Role_Fk_Id = role.ID, ParentId = menu.PARENT_ID, isTrue = (int)BooleanEnum.TRUE });
        }

        int status = 1;
        transaction.Complete();
        transaction.Dispose();
        return await Task.FromResult(status);
    }

    private int SaveRange(List<RoleToMenu> list)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var inserted = 0;
        var timeStamp = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
        var query = @"insert into MISCBILLAPP_ROLE_TO_MENU (role_id_fk, menu_id_fk, is_active,timestamp) values(:ROLE_ID_FK, :MENU_ID_FK, :IS_ACTIVE, " + $" TO_DATE( '" + $"{timeStamp}" + "',  'YYYY-MM-DD HH24:MI:SS')" + @" )";
        inserted += con.Execute(query, list);
        return inserted;
    }

    public async Task<List<Menu>> GetAllMenuHirerchyForRole(string roleId)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"select mn.id,mn.menu_name,mn.url, 
                            case when rtm.is_active is null then 0 else rtm.is_active end is_active,mn.timestamp,mn.icon,mn.parent_id,mn.is_parent
                              from MISCBILLAPP_MENU mn
                              left join MISCBILLAPP_ROLE_TO_MENU rtm
                              on mn.id = rtm.menu_id_fk and rtm.role_id_fk = :RoleFkId
                           where (mn.is_parent=:isTrue and (mn.parent_id is null or mn.parent_id = '0')) 
                            and mn.id <>(select menu_id_fk from MISCBILLAPP_MS_ROLE where id= :RoleFkId) 
                           order by mn.id ";
        var menus = await con.QueryAsync<Menu>(sql, new { RoleFkId = roleId, isTrue = (int)BooleanEnum.TRUE });
        return menus.ToList();
    }

    public async Task<List<Menu>> GetAllRoleToMenuByRoleNUser(int roleId, int userId)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"Select  distinct 
	                            mn.id,
	                            mn.menu_name,
	                            mn.url,
	                            mn.is_parent,
	                            mn.parent_id,
	                            mn.icon,
	                            mn.is_active,
                                mn.is_group,
		                        mn.group_id,
                                MN.ORDER_NO
                            from MISCBILLAPP_ROLE_TO_MENU rtm 
                            left join MISCBILLAPP_MENU mn on rtm.menu_id_fk =  mn.id
                            where (mn.parent_id IS NULL or mn.parent_id = '0')  
	                            and mn.is_parent=:isTrue
	                            and rtm.role_id_fk =:RoleId
	                            and rtm.is_active =:isTrue
	                            
	                      union       
	                      
	                      ( Select  distinct 
	                            mn.id,
	                            mn.menu_name,
	                            mn.url,
	                            mn.is_parent,
	                            mn.parent_id,
	                            mn.icon,
	                            mn.is_active,
                                mn.is_group,
		                        mn.group_id,
                                MN.ORDER_NO
                            from MISCBILLAPP_USER_TO_MENU utm 
                            left join MISCBILLAPP_MENU mn on utm.menu_id_fk = mn.id
                            where (mn.parent_id IS NULL or mn.parent_id = '0')  
	                            and mn.is_parent=:isTrue
	                            and utm.user_id_fk =:UserFkId
	                            and utm.is_active =:isTrue)
                        MINUS
						    (Select  distinct 
	                                mn.id,
	                                mn.menu_name,
	                                mn.url,
	                                mn.is_parent,
	                                mn.parent_id,
	                                mn.icon,
	                                mn.is_active,
                                    mn.is_group,
		                            mn.group_id,
                                    MN.ORDER_NO
                                from MISCBILLAPP_U_TO_RESTRIC_MENU utm 
                                left join MISCBILLAPP_MENU mn on utm.menu_id_fk = mn.id
                                where (mn.parent_id IS NULL or mn.parent_id = '0') 
	                                and mn.is_parent=:isTrue
	                                and utm.user_id_fk =:UserFkId
	                                and utm.is_active =:isTrue )";

        IEnumerable<Menu>? menus = await con.QueryAsync<Menu>(sql, new { RoleId = roleId, UserFkId = userId, isTrue = (int)BooleanEnum.TRUE });
        List<Menu>? t= menus.ToList();
         var r =t.OrderBy(c => c.ORDER_NO).ToList();
        return r;
    }

    public async Task<List<RoleToMenuDto>> GetChildMenuByParentNRole(int roleId, string parentId, int userId)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = $@"(Select  distinct TAB.MenuFkId as MenuFkId, TAB.Name,TAB.ORDER_NO, TAB.Url,TAB.IsParent,TAB.ParentId,TAB.Icon, TAB.IsActive FROM (
                    SELECT rtm.menu_id_fk as MenuFkId , men.menu_name as Name,men.ORDER_NO, men.url as Url,men.is_parent as IsParent ,men.parent_id as ParentId,men.icon as Icon, rtm.is_active as IsActive, rtm.timestamp as TimeStamp
                    FROM MISCBILLAPP_ROLE_TO_MENU rtm
                    inner join MISCBILLAPP_MENU men on men.id= rtm.menu_id_fk
                    Where rtm.role_id_fk= :RoleFkId and men.is_parent =:isFalse and men.parent_id= :ParentId and rtm.is_active=:isTrue
                    union all
                    select utm.menu_id_fk as MenuFkId, men.menu_name as Name ,men.ORDER_NO, men.url as Url, men.is_parent as IsParent, men.parent_id as ParentId, men.icon as Icon, utm.is_active as IsActive, utm.timestamp as TimeStamp from
                    MISCBILLAPP_USER_TO_MENU utm
                    left join MISCBILLAPP_MENU men on men.id = utm.menu_id_fk
                    where utm.is_active= :isTrue and utm.user_id_fk= :UserFkId and men.is_parent =:isFalse and men.parent_id= :ParentId
                    union all
                    select men.id as MenuFkId, men.menu_name as Name ,men.ORDER_NO, men.url as Url,men.is_parent as IsParent,men.parent_id as ParentId,men.icon as Icon, men.is_active as IsActive, men.timestamp as TimeStamp
                    from MISCBILLAPP_MS_ROLE rl
                    left join MISCBILLAPP_MENU men on rl.menu_id_fk=men.id where rl.id=:RoleFkId and men.parent_id= :ParentId
                 ) TAB )

            MINUS

            (select utrm.menu_id_fk as MenuFkId, men.menu_name as Name,men.ORDER_NO, men.url as Url, men.is_parent as IsParent, men.parent_id as ParentId, men.icon as Icon, utrm.is_active as IsActive
					            from MISCBILLAPP_U_TO_RESTRIC_MENU utrm
                                left join MISCBILLAPP_MENU men on men.id= utrm.menu_id_fk
                                where utrm.is_active= :isTrue and utrm.user_id_fk= :UserFkId and men.is_parent = :isFalse  and men.parent_id= :ParentId)";
        var rtmChildList = await con.QueryAsync<RoleToMenuDto>(sql, new { RoleFkId = roleId, ParentId = parentId, UserFkId = userId, isFalse = (int)BooleanEnum.FALSE, isTrue = (int)BooleanEnum.TRUE });

        List<RoleToMenuDto>? t = rtmChildList.ToList();
        var r = t.OrderBy(c => c.ORDER_NO).ToList();
        return r;
    }

    public async Task<List<Menu>> GetChildMenuByParentRTM(string parentId, int roleId)
    { 
        using var con = new OracleConnection(Connection.ConnectionString());
        var sql = @"select mn.id,mn.menu_name,mn.url, 
                            case when rtm.is_active is null then 0 else rtm.is_active end is_active,mn.timestamp,mn.icon,mn.parent_id,mn.is_parent
                              from MISCBILLAPP_MENU mn
                              left join MISCBILLAPP_ROLE_TO_MENU rtm
                              on mn.id = rtm.menu_id_fk and rtm.role_id_fk = :RoleFkId
                           where parent_id=:ParentId and mn.id <> (select menu_id_fk from MISCBILLAPP_MS_ROLE where id = :RoleFkId)
                           order by mn.id ";
        var childMenuList = await con.QueryAsync<Menu>(sql, new
        {
            RoleFkId = roleId,
            ParentId = parentId
        });

        return childMenuList.ToList();
    }
}

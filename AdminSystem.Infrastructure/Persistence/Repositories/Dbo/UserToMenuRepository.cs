using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.Enums;

namespace CFEMS.Infrastructure.Persistence.Repositories.Dbo;

public class UserToMenuRepository : IUserToMenuRepository
{

    
    private IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IMenuRepository _menuRepository;

    public UserToMenuRepository(IMapper mapper, IUserRepository userRepository, IMenuRepository menuRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _menuRepository = menuRepository;
    }

    public async Task<int> AssignUserToMenu(List<UserToMenu> utm)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        using var transaction = new TransactionScope();
        con.ExecuteScalar<int>($"delete from MISCBILLAPP_USER_TO_MENU  where user_id_fk=:user_id_fk and menu_id_fk  in (select id from MISCBILLAPP_MENU)", new { user_id_fk  = utm[0].USER_ID_FK });

        SaveRange(utm);

        int status = 1;
        transaction.Complete();
        transaction.Dispose();

        return await Task.FromResult(status);
    }

    public async Task<int> AssignUserToRestrictedMenu(List<UserToRestrictedMenu> utrmList)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var role = _userRepository.GetRolesByUserId(utrmList[0].USER_ID_FK).Result;
        var menu = _menuRepository.GetByIdAsync(role.MENU_ID_FK).Result;

        using var transaction = new TransactionScope();

        con.ExecuteScalar<int>($"delete from MISCBILLAPP_U_TO_RESTRIC_MENU  where user_id_fk=:user_id_fk and menu_id_fk  in (select id from MISCBILLAPP_MENU)", new { user_id_fk = utrmList[0].USER_ID_FK });

        SaveRangeRestrictedMenu(utrmList);

        if (menu.IS_PARENT != (int)BooleanEnum.TRUE)
        {
            con.ExecuteScalar<int>("update MISCBILLAPP_U_TO_RESTRIC_MENU  set is_active = :isFalse where user_id_fk = :UserFkId and menu_id_fk = :ParentId", new { UserFkId = utrmList[0].USER_ID_FK, ParentId = menu.PARENT_ID, isFalse = (int)BooleanEnum.FALSE });
        }

        int status = 1;
        transaction.Complete();
        transaction.Dispose();

        return await Task.FromResult(status);
    }

    private int SaveRangeRestrictedMenu(List<UserToRestrictedMenu> utrmList)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var inserted = 0;
        var timestamp = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
        var query = @"insert into MISCBILLAPP_U_TO_RESTRIC_MENU (user_id_fk, menu_id_fk, is_active,timestamp) values(:USER_ID_FK,:MENU_ID_FK, :IS_ACTIVE, " + $" TO_DATE( '" + $"{timestamp}" + "',  'YYYY-MM-DD HH24:MI:SS')"+@" )";
        inserted += con.Execute(query, utrmList);

        return inserted;
    }

    private int SaveRange(List<UserToMenu> list)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var inserted = 0;
        var timestamp = DateTime.UtcNow.ToString("yyyy-M-dd hh: mm:ss");
        var query = @"insert into MISCBILLAPP_USER_TO_MENU (user_id_fk,menu_id_fk, is_active,timestamp,is_created, is_edited, is_deleted) values(:USER_ID_FK, :MENU_ID_FK, :IS_ACTIVE,  " + $" TO_DATE( '" + $"{timestamp}" + "',  'YYYY-MM-DD HH24:MI:SS'),"+ @":IS_CREATED, :IS_EDITED,:IS_DELEDTED )"; 
        inserted += con.Execute(query, list);

        return inserted;
    }

    public async Task<List<Menu>> GetAllMenuHirerchyForUser(int userId)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var role = await _userRepository.GetRolesByUserId(userId);

        var sql = @"select mn.id,mn.menu_name,mn.url,  mn.ORDER_NO,
                            case when utm.is_active is null then 0 else utm.is_active end is_active,mn.timestamp,mn.icon,mn.parent_id,mn.is_parent
                              from MISCBILLAPP_MENU mn
                              left join MISCBILLAPP_USER_TO_MENU utm
                              on mn.id = utm.menu_id_fk and utm.user_id_fk = :UserFkId
                           where (mn.is_parent=:isTrue and (mn.parent_id is null or mn.parent_id = '0')) 
                       --  and  mn.id <> (select menu_id_fk from MISCBILLAPP_MS_ROLE where id = :RoleId) 
                           order by mn.ORDER_NO ASC ";
        var menus = await con.QueryAsync<Menu>(sql, new { UserFkId = userId, RoleId = role.ID, isTrue = (int)BooleanEnum.TRUE });
        return menus.ToList();
    }

    public async Task<List<Menu>> GetChildMenuByParentUTM(string parentId, int userId)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var role = await _userRepository.GetRolesByUserId(userId);

        var sql = @"select mn.id,mn.menu_name,mn.url, mn.ORDER_NO,utm.IS_CREATED,utm.IS_EDITED,utm.IS_DELETED,
                            case when utm.is_active is null then 0 else utm.is_active end is_active,mn.timestamp,mn.icon,mn.parent_id,mn.is_parent
                              from MISCBILLAPP_MENU mn
                              left join MISCBILLAPP_USER_TO_MENU utm
                              on mn.id = utm.menu_id_fk and utm.user_id_fk = :UserFkId
                           where parent_id=:ParentId
                           order by  mn.ORDER_NO ASC ";
        //select mn.id,mn.menu_name,mn.url, mn.ORDER_NO,
        //                            case when utm.is_active is null then 0 else utm.is_active end is_active,mn.timestamp,mn.icon,mn.parent_id,mn.is_parent
        //                              from MISCBILLAPP_MENU mn
        //                              left join MISCBILLAPP_USER_TO_MENU utm
        //                              on mn.id = utm.menu_id_fk and utm.user_id_fk = :UserFkId
        //                           where parent_id=:ParentId
        //                           --and mn.id <> (select menu_id_fk from MISCBILLAPP_MS_ROLE where id = :RoleId)
        //                           order by  mn.ORDER_NO ASC
        var childMenuList = await con.QueryAsync<Menu>(sql, new { UserFkId = userId, ParentId = parentId, RoleId = role.ID });

        return childMenuList.ToList();
    }

    public async Task<List<Menu>> GetAllRestrictedMenuHirerchyForUser(int userId)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var role = await _userRepository.GetRolesByUserId(userId);

        var sql = @"select mn.id,mn.menu_name,mn.url, 
                            case when utrm.is_active is null then 0 else utrm.is_active end is_active,mn.timestamp,mn.icon,mn.parent_id,mn.is_parent
                              from MISCBILLAPP_MENU mn
                              left join MISCBILLAPP_U_TO_RESTRIC_MENU utrm
                              on mn.id = utrm.menu_id_fk and utrm.user_id_fk = :UserFkId
                           where (mn.is_parent=:isTrue and (mn.parent_id is null or mn.parent_id = '0')) and mn.id <>(select menu_id_fk from MISCBILLAPP_MS_ROLE where id = :RoleId) 
                           order by mn.id ";
        var menus = await con.QueryAsync<Menu>(sql, new { UserFkId = userId.ToString(), RoleId = role.ID, isTrue = (int)BooleanEnum.TRUE });
        return menus.ToList();
    }

    public async Task<List<Menu>> GetChildRestrictedMenuByParentUTM(string parentId, int userId)
    {
        using var con = new OracleConnection(Connection.ConnectionString());
        var role = await _userRepository.GetRolesByUserId(userId);

        var sql = @"select mn.id,mn.menu_name,mn.url, 
                            case when utrm.is_active is null then 0 else utrm.is_active end is_active,mn.timestamp,mn.icon,mn.parent_id,mn.is_parent
                              from MISCBILLAPP_MENU mn
                              left join MISCBILLAPP_U_TO_RESTRIC_MENU utrm
                              on mn.id = utrm.menu_id_fk and utrm.user_id_fk = :UserFkId
                           where parent_id=:ParentId and mn.id <> (select menu_id_fk from MISCBILLAPP_MS_ROLE where id = :RoleId)
                           order by mn.id ";
        var childMenuList = await con.QueryAsync<Menu>(sql, new { UserFkId = userId.ToString(), ParentId = parentId, RoleId = role.ID });

        return childMenuList.ToList();
    }
}

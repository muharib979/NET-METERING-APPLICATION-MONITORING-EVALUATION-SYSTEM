using AdminSystem.Infrastructure.Persistence.Context;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Dbo
{
    public class UserAccessMenuRepository : IUserAccessMenuRepository
    {
        public async Task<List<UserAccessMenuDTO>> GetUserMenuAccess(int pageId, string userName)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var sQuery = $@"SELECT UD.USER_NAME UserName,M.ID PageId,M.MENU_NAME MenuName,
                        UM.IS_CREATED IsCreated,UM.IS_EDITED IsEdited ,
                         UM.IS_DELETED IsDeleted FROM MISCBILLAPP_USER_DETAILS UD 
                         INNER JOIN MISCBILLAPP_USER_TO_MENU UM ON UD.ID=UM.USER_ID_FK
                         INNER JOIN MISCBILLAPP_MENU M ON UM.MENU_ID_FK=M.ID WHERE M.ID=:pageId
                         AND UD.USER_NAME=:userName";
            var result = await con.QueryAsync<UserAccessMenuDTO>(sQuery, new { PageId = pageId, UserName = userName });
            return result.ToList();
        }
    }
}

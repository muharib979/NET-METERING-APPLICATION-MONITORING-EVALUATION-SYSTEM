using Core.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Dbo.RepositoryInterfaces
{
    public interface IUserAccessMenuRepository
    {
        Task<List<UserAccessMenuDTO>> GetUserMenuAccess(int pageId, string userName);
    }
}

using Core.Domain.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface
{
    public interface IProsoftUserRepository : IBaseRepository<ProsoftUsers>
    {
        Task<int> AddProsoftUser(ProsoftUsers users);
        Task<ProsoftUsers> GetUserByName(string name);
        Task<int> UpdateUserTokenInfo(ProsoftUsers users);
        Task<ProsoftUsers> GetByRefreshtokenAsync(string refreshToken);
        Task<ProsoftUsers> GetUserByIdAsync(int userid);

    }
}

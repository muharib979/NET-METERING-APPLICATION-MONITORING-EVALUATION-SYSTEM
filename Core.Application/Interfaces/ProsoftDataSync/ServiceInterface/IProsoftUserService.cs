using Core.Application.Commands.ProsoftDataSync.CreateUser;
using Core.Domain.ProsoftDataSync;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ProsoftDataSync.ServiceInterface
{
    public interface IProsoftUserService : IBaseService<ProsoftUserDTO>
    {
        Task<int> AddProsoftUser(CreateUserCommand command);
        Task<ProsoftUsers> GetUserByName(string name);
        Task<int> UpdateUserTokenInfo(ProsoftUsers users);

        Task<ProsoftUsers> GetByRefreshtokenAsync(string refreshToken);
        Task<ProsoftUsers> GetUserByIdAsync(int userid);
    }
}

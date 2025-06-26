using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface
{
    public interface IProsoftTokenRepository : IBaseRepository<Token>
    {
        Task<Token> GetByRefreshtokenAsync(string refreshToken);
        Task<int> DeleteByUserIdAsync(string userId);
        Task<int> DeleteExpireRefreshToken();
        Task<Token> GetBySessionIdAsync(string sessionId);
    }
}

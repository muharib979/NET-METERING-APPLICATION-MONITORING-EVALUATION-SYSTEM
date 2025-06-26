namespace Core.Application.Interfaces.Dbo.RepositoryInterfaces;

public interface ITokenRepository : IBaseRepository<Token>
{
    Task<Token> GetByRefreshtokenAsync(string refreshToken);
    Task<int> DeleteByUserIdAsync(int userId);
    Task<int> DeleteExpireRefreshToken();
    Task<Token> GetBySessionIdAsync(string sessionId);
}

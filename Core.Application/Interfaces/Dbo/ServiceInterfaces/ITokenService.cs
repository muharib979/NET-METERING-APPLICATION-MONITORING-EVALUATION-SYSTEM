using Core.Application.Services.Dbo;

namespace Core.Application.Interfaces.Dbo.ServiceInterfaces;

public interface ITokenService
{
    public Task<TokenService> UserValidate(TokenRequestDto requestDto);
    public Task<TokenService> UserValidateById();
    public Task<TokenService> CreateRefreshtoken(string sessionId);
    public Task<TokenResponseDto> CreateAccessToken();
    public Task<TokenService> RefreshTokenValidate(TokenRequestDto tokenRequestDto);
    public Task<TokenService> IsCurrentSession(string sessionId);
    public Task<TokenService> DeleteTokenByUserId(string userName);
    public Task<int> DeleteExpireRefreshToken();
}

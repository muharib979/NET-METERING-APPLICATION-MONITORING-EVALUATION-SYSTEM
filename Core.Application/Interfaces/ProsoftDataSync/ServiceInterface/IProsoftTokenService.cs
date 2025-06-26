using Core.Application.Services.ProsoftDataSync;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ProsoftDataSync.ServiceInterface
{
    public interface IProsoftTokenService
    {
        public Task<ProsoftTokenService> UserValidate(ProsoftTokenRequestDto requestDto);
        public Task<ProsoftTokenService> UserValidateById();
        public Task<ProsoftTokenService> CreateRefreshtoken(string sessionId);
        public Task<ProsoftTokenResponseDto> CreateAccessToken();
        public Task<ProsoftTokenService> RefreshTokenValidate(RefreshTokenDTO tokenRequestDto);
        public Task<ProsoftTokenService> IsCurrentSession(string sessionId);
        public Task<ProsoftTokenService> DeleteTokenByUserId(string userName);
        public Task<int> DeleteExpireRefreshToken();
    }
}

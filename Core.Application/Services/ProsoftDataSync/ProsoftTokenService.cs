using Core.Application.Common.Exceptions;
using Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface;
using Core.Application.Interfaces.ProsoftDataSync.ServiceInterface;
using Core.Domain.ProsoftDataSync;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services.ProsoftDataSync
{
    public class ProsoftTokenService : IProsoftTokenService
    {
        private readonly IProsoftUserService _userService;
        private readonly IConfiguration _config;
        private readonly IProsoftTokenRepository _tokenRepository;
        //private readonly IMenuService _menuService;
        private readonly IMenuRepository _menurepository;
        private readonly SymmetricSecurityKey _key;
        private ProsoftUsers user;
        private Token newRefreshToken;
        private Token refreshToken;
        private bool IsDeleted;

        public ProsoftTokenService(IProsoftUserService userService, IConfiguration config, IProsoftTokenRepository tokenRepository, IMenuRepository menurepository)
        {
            _userService = userService;
            _config = config;
            _tokenRepository = tokenRepository;
            //_menuService = menuService;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecuritySettings:JwtSettings:key"]));
            _menurepository = menurepository;
        }
        public async Task<ProsoftTokenService> UserValidate(ProsoftTokenRequestDto requestDto)
        {
            var userFromDb = await _userService.GetUserByName(requestDto.UserName);
            if (userFromDb == null) throw new AppException("Invalid User Name");

            using var hmac = new HMACSHA512(Convert.FromBase64String(userFromDb.PASSWORD_SALT));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestDto.Password));

            if (Convert.ToBase64String(computedHash) != userFromDb.PASSWORD) throw new AppException("Invalid Password");

            user = userFromDb;

            return this;
        }

        public async Task<ProsoftTokenService> CreateRefreshtoken()
        {
            //newRefreshToken = new Token()
            //{
            //    col_user_id = user.col_user_id.ToString(),
            //    col_value = Guid.NewGuid().ToString("N"),
            //    col_created_date = DateTime.Now,
            //    col_expiry_time = DateTime.Now.AddMinutes(double.Parse(_config["SecuritySettings:JwtSettings:refreshTokenExpirationInMinutes"]))
            //};

            user.TOKEN = Guid.NewGuid().ToString("N");
            user.TOKEN_CREATEDATE = DateTime.Now;
            user.TOKEN_EXPIRYDATE = DateTime.Now.AddMinutes(double.Parse(_config["SecuritySettings:JwtSettings:refreshTokenExpirationInMinutes"]));
            user.TOKEN_MODIFYDATE = DateTime.Now;

            await _userService.UpdateUserTokenInfo(user);

            //await _tokenRepository.DeleteByUserIdAsync(user.col_user_id.ToString());
            //await _tokenRepository.AddAsync(newRefreshToken);

            return this;
        }

        public async Task<ProsoftTokenService> CreateRefreshtoken(string sessionId)
        {
            //newRefreshToken = new Token()
            //{
            //    col_user_id = user.col_user_id.ToString(),
            //    col_value = Guid.NewGuid().ToString("N"),
            //    col_created_date = DateTime.Now,
            //    col_expiry_time = DateTime.Now.AddMinutes(double.Parse(_config["SecuritySettings:JwtSettings:refreshTokenExpirationInMinutes"])),
            //    col_session_id = sessionId
            //};

            //var getTokenBySessionIdFromDb = await _tokenRepository.GetBySessionIdAsync(sessionId);

            //if (getTokenBySessionIdFromDb != null && getTokenBySessionIdFromDb.col_session_id == sessionId)
            //{
            //    await _tokenRepository.DeleteByUserIdAsync(user.col_user_id.ToString());
            //}

            ////await _tokenRepository.DeleteByUserIdAndSessionIdAsync(user.col_user_id.ToString(),sessionId);


            //await _tokenRepository.AddAsync(newRefreshToken);
            user.TOKEN = Guid.NewGuid().ToString("N");
            user.TOKEN_CREATEDATE = DateTime.Now;
            user.TOKEN_EXPIRYDATE = DateTime.Now.AddMinutes(double.Parse(_config["SecuritySettings:JwtSettings:refreshTokenExpirationInMinutes"]));
            user.TOKEN_MODIFYDATE = DateTime.Now;

            await _userService.UpdateUserTokenInfo(user);

            return this;
        }

        public async Task<ProsoftTokenResponseDto> CreateAccessToken()
        {
            double tokenExpiryTime = double.Parse(_config["SecuritySettings:JwtSettings:tokenExpirationInMinutes"]);

            var key = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);


            var tokenHandler = new JwtSecurityTokenHandler();

            //var role = await _userService.GetRoleByUserId(user.col_user_id.ToString());
            //var roleDashboard = await _menuService.GetByIdAsync(Guid.Parse(role.col_menu_id_fk));
            var claims = new List<Claim>();

            //if (role != null)
            //{
            //    claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.col_user_name));
            //    claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.col_user_id.ToString()));
            //    //claims.Add(new Claim(ClaimTypes.Role, role.col_role_name));
            //    //claims.Add(new Claim("roleid", role.col_role_id.ToString()));
            //    claims.Add(new Claim("roleid", role.col_role_id.ToString()));
            //}
            //else
            //{
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.USER_NAME));
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, "ProsoftUser"));
            //}

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = key,
                Expires = DateTime.Now.AddMinutes(tokenExpiryTime)
            };

            var newtoken = tokenHandler.CreateToken(tokenDescriptor);

            var encodedToken = tokenHandler.WriteToken(newtoken);

            return new ProsoftTokenResponseDto()
            {
                Token = encodedToken,
                Expiration = DateTime.Now.AddMinutes(tokenExpiryTime), //newtoken.ValidTo,
                RefreshToken = user.TOKEN,
                RefreshTokenExpiration = user.TOKEN_EXPIRYDATE
            };
        }

        public async Task<ProsoftTokenService> RefreshTokenValidate(RefreshTokenDTO tokenRequestDto)
        {
            var refreshTokenFromDb = await _userService.GetByRefreshtokenAsync(tokenRequestDto.RefreshToken); //await _tokenRepository.GetByRefreshtokenAsync(tokenRequestDto.RefreshToken);
            if (refreshTokenFromDb == null) throw new UnauthorizedAccessException("Refresh Token Invalid");

            var refreshTokenExpiryTime = refreshTokenFromDb.TOKEN_EXPIRYDATE;
            if (refreshTokenExpiryTime < DateTime.Now) throw new UnauthorizedAccessException("Refresh Token Expired");

            user = refreshTokenFromDb;

            return this;
        }

        public async Task<ProsoftTokenService> UserValidateById()
        {
            try
            {
                var userFromDb = await _userService.GetUserByIdAsync(user.ID);
                if (userFromDb == null) throw new UnauthorizedAccessException("User Invalid");

                user = userFromDb;
                return this;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ProsoftTokenService> IsCurrentSession(string sessionId)
        {
            var sessionIdFromExistingRefreshToken = refreshToken.SESSION_ID;
            if (sessionIdFromExistingRefreshToken != sessionId) throw new UnauthorizedAccessException("Session Ended");
            return this;
        }

        public async Task<ProsoftTokenService> DeleteTokenByUserId(string userName)
        {
            //if (!string.IsNullOrEmpty(userName))
            //{
            //    var userFromDb =  await _userService.GetByNameAsync(userName);
            //    var deleted = await _tokenRepository.DeleteByUserIdAsync(userFromDb.col_user_id.ToString());
            //    if (deleted < 1) throw new UnauthorizedAccessException("Couldn't Delete");
            //    IsDeleted = true;
            //}

            return this;
        }

        public async Task<int> DeleteExpireRefreshToken() => await _tokenRepository.DeleteExpireRefreshToken();
    }
}
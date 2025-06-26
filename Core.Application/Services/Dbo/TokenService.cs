using Core.Application.Common.Exceptions;
using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Core.Application.Interfaces.Dbo.ServiceInterfaces;
using Core.Application.Interfaces.VisitorDetails;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.CustomerDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Application.Services.Dbo;

public class TokenService : ITokenService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;
    private readonly ITokenRepository _tokenRepository;
    private readonly IMenuService _menuService;
    private readonly IVisitorDetails _visitorDetails;
    private readonly SymmetricSecurityKey _key;
    private User user;
    private Token newRefreshToken;
    private Token refreshToken;
    private bool IsDeleted;

    public TokenService(IUserService userService, IConfiguration config, ITokenRepository tokenRepository, IMenuService menuService, IVisitorDetails visitorDetails)
    {
        _userService = userService;
        _config = config;
        _tokenRepository = tokenRepository;
        _menuService = menuService;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecuritySettings:JwtSettings:key"]));
        _visitorDetails = visitorDetails;
    }
    public async Task<TokenService> UserValidate(TokenRequestDto requestDto)
    {
        
        var userFromDb = await _userService.GetByNameAsync(requestDto.UserName);

        //if (userFromDb == null)
        //{
        //    var errorResponse = new ErrorResponse
        //    {
        //        Status = 401,
        //        Data = new { }, // Empty object
        //        Errors = new List<ErrorDetail>
        //    {
        //        new ErrorDetail { Code = "401.1", Message = "Username or password is invalid." }
        //    }
        //    };

        //    throw new AppException(errorResponse);
        //}


        //if (userFromDb == null) throw new AppException("Invalid User Name", TokenResponseDto);

        using var hmac = new HMACSHA512(Convert.FromBase64String(userFromDb.PASSWORD_SALT));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestDto.Password));

        if (Convert.ToBase64String(computedHash) != userFromDb.PASSWORD) throw new AppException("Invalid Password");

        user = userFromDb;


        return this;
        //return ResponseNem<TokenResponseDto>.Fail(""); 
    }

    public async Task<TokenService> CreateRefreshtoken()
    {
        newRefreshToken = new Token()
        {
            ID = user.ID,
            VALUE = Guid.NewGuid().ToString("N"),
            CREATED_DATE = DateTime.Now,
            EXPIRY_TIME = DateTime.Now.AddMinutes(double.Parse(_config["SecuritySettings:JwtSettings:refreshTokenExpirationInMinutes"]))
        };

        await _tokenRepository.DeleteByUserIdAsync(user.ID);
        await _tokenRepository.AddAsync(newRefreshToken);

        return this;
    }

    public async Task<TokenService> CreateRefreshtoken(string sessionId)
    {
        newRefreshToken = new Token()
        {
            USER_ID = user.ID,
            VALUE = Guid.NewGuid().ToString("N"),
            CREATED_DATE = DateTime.Now,
            EXPIRY_TIME = DateTime.Now.AddMinutes(double.Parse(_config["SecuritySettings:JwtSettings:refreshTokenExpirationInMinutes"])),
            SESSION_ID = sessionId
        };

        var getTokenBySessionIdFromDb = await _tokenRepository.GetBySessionIdAsync(sessionId);

        if (getTokenBySessionIdFromDb != null && getTokenBySessionIdFromDb.SESSION_ID == sessionId)
        {
            await _tokenRepository.DeleteByUserIdAsync(user.ID);
        }

        //await _tokenRepository.DeleteByUserIdAndSessionIdAsync(user.col_user_id.ToString(),sessionId);


        await _tokenRepository.AddAsync(newRefreshToken);

        return this;
    }

    public async Task<TokenResponseDto> CreateAccessToken()
    {
        double tokenExpiryTime = double.Parse(_config["SecuritySettings:JwtSettings:tokenExpirationInMinutes"]);

        var key = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);


        var tokenHandler = new JwtSecurityTokenHandler();

        var role = await _userService.GetRoleByUserId(user.ID);
        var roleDashboard = await _menuService.GetByIdAsync(role.MENU_ID_FK);
        var dbCodeList = await _userService.GetDbCodeByUserId(user.ID);
        var locationCodeList = await _userService.GetLocationCode(user.ID);

        var claims = new List<Claim>();

        if (role != null)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.USER_NAME));
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, role.ROLE_NAME));
            claims.Add(new Claim("roleid", role.ID.ToString()));
        }
        else
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.USER_NAME));
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.ID.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = key,
            Expires = DateTime.Now.AddMinutes(tokenExpiryTime)
        };

        var newtoken = tokenHandler.CreateToken(tokenDescriptor);

        var encodedToken = tokenHandler.WriteToken(newtoken);

        return new TokenResponseDto()
        {


            Token = encodedToken,
            ExpiresIn = 900,
            RefreshToken = newRefreshToken.VALUE,

            CreateTimestamp = DateTime.Now,

            //Token = encodedToken,
            //Expiration = newtoken.ValidTo,
            //RefreshToken = newRefreshToken.VALUE,
            //RefreshTokenExpiration = newRefreshToken.EXPIRY_TIME,
            //UserName = user.USER_NAME,
            //FirstName = user.USER_NAME,
            //LastName = "",
            //RoleName = role == null ? "" : role.ROLE_NAME,
            //Email = user.EMAIL,
            //DashBoardUrl = roleDashboard == null ? "/quick-access/dashboard" : roleDashboard.Url ?? "",
            //DbCodeList = dbCodeList,
            //LocationCodeList = locationCodeList

        };
    }

    public async Task<TokenService> RefreshTokenValidate(TokenRequestDto tokenRequestDto)
    {
        //var refreshTokenFromDb = await _tokenRepository.GetByRefreshtokenAsync(tokenRequestDto.RefreshToken);
        var refreshTokenFromDb = await _tokenRepository.GetByRefreshtokenAsync("");
        if (refreshTokenFromDb == null) throw new UnauthorizedAccessException("Refresh Token Invalid");

        var refreshTokenExpiryTime = refreshTokenFromDb.EXPIRY_TIME;
        if (refreshTokenExpiryTime < DateTime.UtcNow) throw new UnauthorizedAccessException("Refresh Token Expired");

        refreshToken = refreshTokenFromDb;

        return this;
    }

    public async Task<TokenService> UserValidateById()
    {
        var userFromDb = await _userService.GetByIdAsync(refreshToken.USER_ID);
        if (userFromDb == null) throw new UnauthorizedAccessException("User Invalid");

        user = userFromDb;

        return this;
    }

    public async Task<TokenService> IsCurrentSession(string sessionId)
    {
        var sessionIdFromExistingRefreshToken = refreshToken.SESSION_ID;
        if (sessionIdFromExistingRefreshToken != sessionId) throw new UnauthorizedAccessException("Session Ended");
        return this;
    }

    public async Task<TokenService> DeleteTokenByUserId(string userName)
    {
        if (!string.IsNullOrEmpty(userName))
        {
            var userFromDb = await _userService.GetByNameAsync(userName);
            var deleted = await _tokenRepository.DeleteByUserIdAsync(userFromDb.ID);
            if (deleted < 1) throw new UnauthorizedAccessException("Couldn't Delete");
            IsDeleted = true;
        }

        return this;
    }

    public async Task<int> DeleteExpireRefreshToken() => await _tokenRepository.DeleteExpireRefreshToken();
}

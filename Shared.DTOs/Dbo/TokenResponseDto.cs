using Shared.DTOs.CustomerDto;

namespace Shared.DTOs.Dbo;

public class TokenResponseDto
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    //public DateTime RefreshTokenExpiration { get; set; }
    public DateTime CreateTimestamp { get; set; }


    //public string Token { get; set; }
    //public DateTime Expiration { get; set; }
    //public string RefreshToken { get; set; }
    //public DateTime RefreshTokenExpiration { get; set; }
    //public string FirstName { get; set; }
    //public string LastName { get; set; }
    //public string UserName { get; set; }
    //public string Email { get; set; }
    //public string RoleName { get; set; }
    //public string DashBoardUrl { get; set; }
    //public List<string> DbCodeList { get; set; }
    //public List<string> LocationCodeList { get; set; }
  

}

public class TokenResponseDtos
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    //public DateTime RefreshTokenExpiration { get; set; }
    public DateTime CreateTimestamp { get; set; }
    //    public int Status { get; set; }
    //    public TokenResponseDto Data { get; set; }
    //    public List<ErrorDetail> Errors { get; set; }
    //}
}

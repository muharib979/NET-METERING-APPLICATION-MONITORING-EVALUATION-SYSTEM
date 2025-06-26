namespace Shared.DTOs.Dbo;

public class UserDto
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public string SmsPhoneNbr { get; set; }
    public int TotalRowCount { get; set; }
}
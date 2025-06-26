using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.GetVerificationCode;

public class GetVerificationCodeCommand : IRequest<Response<bool>>
{
    public string Email { get; set; }
}

using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.UserLogin;

public class UserLoginCommand : TokenRequestDto, IRequest<ResponseNem<TokenResponseDto>> { }

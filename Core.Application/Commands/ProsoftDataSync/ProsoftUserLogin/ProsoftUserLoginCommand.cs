using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.ProsoftDataSync;


namespace Core.Application.Commands.ProsoftDataSync.ProsoftUserLogin
{
    public class ProsoftUserLoginCommand : ProsoftTokenRequestDto, IRequest<Response<ProsoftTokenResponseDto>>{ }
}

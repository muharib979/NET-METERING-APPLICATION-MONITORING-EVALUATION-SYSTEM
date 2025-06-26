using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.DeleteExpireRefreshToken;

public class DeleteExpireRefreshTokenCommandHandler : IRequestHandler<DeleteExpireRefreshTokenCommand, Response<IActionResult>>
{
    private readonly ITokenService _tokenService;
    public DeleteExpireRefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    public async Task<Response<IActionResult>> Handle(DeleteExpireRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _tokenService.DeleteExpireRefreshToken();
         return result > 0? Response <IActionResult>.Success("Expired Refresh Tokens Deleted Successfullly") :  Response<IActionResult>.Fail("Failed To Expired Refresh Tokens");
    }
}

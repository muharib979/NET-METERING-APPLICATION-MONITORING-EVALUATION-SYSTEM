using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.OffiecStuff;

namespace Core.Application.Commands.OfficeStuff.AddOfficeStuff
{
    public class AddOfficeStuffCommand : OfficeStuffDto, IRequest<Response<IActionResult>>
    {
    }
}

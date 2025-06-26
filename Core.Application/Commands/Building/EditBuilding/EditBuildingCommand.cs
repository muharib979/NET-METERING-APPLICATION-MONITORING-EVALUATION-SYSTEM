using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Shared.DTOs.Building;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Building.EditBuilding;

public class EditBuildingCommand : BuildingDto, IRequest<Response<IActionResult>>
{
    public class Handler : IRequestHandler<EditBuildingCommand, Response<IActionResult>>
    {
        private readonly IBuildingRepository _repository;
        public Handler(IBuildingRepository repository) 
        {
            _repository= repository;
        }
        public async Task<Response<IActionResult>> Handle(EditBuildingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int result = await _repository.UpdateAsync(request);
                return result > 0 ? Response<IActionResult>.Success("Building Edited Successfully") : Response<IActionResult>.Fail("Problem Saving Changes");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Response<IActionResult>.Fail("Problem Saving Changes");
            }
        }
    }
}

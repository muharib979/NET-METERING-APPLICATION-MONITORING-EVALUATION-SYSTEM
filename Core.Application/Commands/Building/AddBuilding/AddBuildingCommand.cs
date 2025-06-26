using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Shared.DTOs.Building;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Building.AddBuilding;

public class AddBuildingCommand: BuildingDto, IRequest<Response<IActionResult>>
{
    public class Handler : IRequestHandler<AddBuildingCommand, Response<IActionResult>>
    {
        private readonly IBuildingRepository _repository;
        public Handler(IBuildingRepository repository) 
        {
            _repository= repository;
        }
        public async Task<Response<IActionResult>> Handle(AddBuildingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int result = await _repository.AddAsync(request);
                return result > 0 ? Response<IActionResult>.Success("Building Added Successfully") : Response<IActionResult>.Fail("Problem Saving Changes");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Response<IActionResult>.Fail("Problem Saving Changes");
            }
        }
    }
}

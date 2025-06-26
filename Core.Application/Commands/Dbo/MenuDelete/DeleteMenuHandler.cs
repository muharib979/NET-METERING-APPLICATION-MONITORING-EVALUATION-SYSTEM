using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.UserAddByCenterLocation
{
    public class DeleteMenuHandler : IRequest<Response<IActionResult>>
    {
        public int Id { get; set; }
        public class Handler : IRequestHandler<DeleteMenuHandler, Response<IActionResult>>
        {
            private readonly IMenuRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IMenuRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<IActionResult>> Handle(DeleteMenuHandler request, CancellationToken cancellationToken)
            {
                var user = await _repository.DeleteMenuById(request.Id);

                return Response<IActionResult>.Success("Successfully Delete User");
            }
        }

    }
}

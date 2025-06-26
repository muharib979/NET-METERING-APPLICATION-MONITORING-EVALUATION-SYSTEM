using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.RoleGetById;

public class RoleGetByIdQuery : IRequest<Response<RoleDto>>
{
    public int Id { get; set; }

    public class Handler : IRequestHandler<RoleGetByIdQuery, Response<RoleDto>>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        public Handler(IRoleRepository repository, IMapper mapper) 
        {
            _repository= repository;
            _mapper= mapper;
        }
        public async Task<Response<RoleDto>> Handle(RoleGetByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _repository.GetByIdAsync(request.Id);
            if (role == null) return Response<RoleDto>.Fail("Role does not exists");
            var result = _mapper.Map<RoleDto>(role);
            return Response<RoleDto>.Success(result, "Successfully Retrived Role by Id");
        }
    }
}

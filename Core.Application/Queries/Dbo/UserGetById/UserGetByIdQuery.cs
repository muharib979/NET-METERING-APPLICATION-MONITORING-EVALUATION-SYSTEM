using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.UserGetById;

public class UserGetByIdQuery : IRequest<Response<UserDto>>
{
    public int Id { get; set; }
    public class Handler : IRequestHandler<UserGetByIdQuery, Response<UserDto>>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Response<UserDto>> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);
            if (user == null) return Response<UserDto>.Fail("User does not exists");
            var result = _mapper.Map<UserDto>(user);
            return Response<UserDto>.Success(result, "Successfully Retrived User By Id");
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Dbo.UserDDList
{
    public class GetAllUserDdByPriorityAndUserQuery: IRequest<List<DropdownResult>>
    {
        public string UserName { get; set; }
        public string LocationCode { get; set; }

        public class Handler : IRequestHandler<GetAllUserDdByPriorityAndUserQuery, List<DropdownResult>>
        {
            private readonly IUserRepository _userRepository;
            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<List<DropdownResult>> Handle(GetAllUserDdByPriorityAndUserQuery request, CancellationToken cancellationToken)
            {
                var result = await _userRepository.GetAllUserDdByPriorityAndUser(request.UserName, request.LocationCode);
                return result;
            }
        }
    }
}

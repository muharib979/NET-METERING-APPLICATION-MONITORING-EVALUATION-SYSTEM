using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Dbo.UserAccessMenuList
{
    public class UserAccessMenuQueryList : IRequest<List<UserAccessMenuDTO>>
    {
        public int PageId { get; set; }
        public string UserName { get; set; }
        public class Handler : IRequestHandler<UserAccessMenuQueryList, List<UserAccessMenuDTO>>
        {
            private readonly IUserAccessMenuRepository _repository;

            public Handler(IUserAccessMenuRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<UserAccessMenuDTO>> Handle(UserAccessMenuQueryList request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetUserMenuAccess(request.PageId,request.UserName);
                return result;
            }
        }
    }
}

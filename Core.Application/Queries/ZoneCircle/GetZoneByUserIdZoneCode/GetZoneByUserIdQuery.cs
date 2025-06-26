using Core.Application.Interfaces.ZoneCircle;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.ZoneCircle.GetZoneByUserId
{
    public class GetZoneByUserIdQuery:IRequest<List<ZoneDto>>
    {
        public string UserName { get; set; }
        public class Handler : IRequestHandler<GetZoneByUserIdQuery, List<ZoneDto>>
        {
            private readonly IZoneCircleRepository _repository;
            public Handler(IZoneCircleRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<ZoneDto>> Handle(GetZoneByUserIdQuery request, CancellationToken cancellationToken)
            {
                var result=await _repository.GetZoneByUserID(request.UserName);
                return result;
            }
        }
    }
}

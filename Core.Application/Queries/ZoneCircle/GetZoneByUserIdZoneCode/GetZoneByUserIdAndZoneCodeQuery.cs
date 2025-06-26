using Core.Application.Interfaces.ZoneCircle;
using Core.Application.Queries.ZoneCircle.GetZoneByUserId;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.ZoneCircle.GetZoneByUserIdZoneCode
{
    public class GetZoneByUserIdAndZoneCodeQuery : IRequest<List<CircleDTO>>
    {
        public string UserName { get; set; }
        public string ZoneCode { get; set; }
        public class Handler : IRequestHandler<GetZoneByUserIdAndZoneCodeQuery, List<CircleDTO>>
        {
            private readonly IZoneCircleRepository _repository;
            public Handler(IZoneCircleRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<CircleDTO>> Handle(GetZoneByUserIdAndZoneCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetCircleByUserNameZoneCode(request.UserName, request.ZoneCode);
                return result;
            }
        }
    }
}

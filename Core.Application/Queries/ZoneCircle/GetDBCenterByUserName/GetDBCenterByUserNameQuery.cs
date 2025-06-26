using Core.Application.Interfaces.ZoneCircle;
using Shared.DTOs.DatabaseConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.ZoneCircle.GetDBCenterByUserName
{
    public class GetDBCenterByUserNameQuery:IRequest<List<DatabaseConfigDto>>
    {
        public string UserName { get; set; }

        public class Handler : IRequestHandler<GetDBCenterByUserNameQuery, List<DatabaseConfigDto>>{
            private readonly IZoneCircleRepository _zoneCircleRepository;
            public Handler(IZoneCircleRepository zoneCircleRepository)
            {
                _zoneCircleRepository = zoneCircleRepository;
            }

            public async Task<List<DatabaseConfigDto>> Handle(GetDBCenterByUserNameQuery request, CancellationToken cancellationToken)
            {
                var result = await _zoneCircleRepository.GetDBCenterByUserName(request.UserName);
                return result;
            }
        }
    }
}

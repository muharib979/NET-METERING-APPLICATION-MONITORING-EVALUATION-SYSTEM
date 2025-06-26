using Core.Application.Interfaces.Location;
using Core.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig.GetLocByDbCodeUserIdRoleId
{
    public class GetLocationByMultipleDbCodeUserIdRoleIdQuery : IRequest<List<Locations>>
    {
        public string[] DbCodes { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public class Handler : IRequestHandler<GetLocationByMultipleDbCodeUserIdRoleIdQuery, List<Locations>>
        {
            private readonly ILocationRepository _locationRepository;
            public Handler(ILocationRepository locationRepository)
            {
                _locationRepository = locationRepository;
            }

            public async Task<List<Locations>> Handle(GetLocationByMultipleDbCodeUserIdRoleIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _locationRepository.GetLocationByMultipleDBUserRoleId(request.DbCodes, request.UserId, request.RoleId);
                return result;
            }
        }
    }
}

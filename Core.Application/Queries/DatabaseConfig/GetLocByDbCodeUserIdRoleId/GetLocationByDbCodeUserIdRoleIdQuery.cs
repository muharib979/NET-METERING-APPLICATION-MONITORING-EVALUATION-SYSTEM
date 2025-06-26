using Core.Application.Interfaces.Location;
using Core.Application.Queries.DatabaseConfig.GetDBbyUserId;
using Core.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.DatabaseConfig.GetLocByDbCodeUserIdRoleId
{
    public class GetLocationByDbCodeUserIdRoleIdQuery:IRequest<List<Locations>>
    {
        public string DbCode { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public class Handler : IRequestHandler<GetLocationByDbCodeUserIdRoleIdQuery, List<Locations>>
        {
            private readonly ILocationRepository _locationRepository;
            public Handler( ILocationRepository locationRepository)
            {
                _locationRepository = locationRepository;
            }

            public async Task<List<Locations>> Handle(GetLocationByDbCodeUserIdRoleIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _locationRepository.GetLocationByDBMIdAsync(request.DbCode,request.UserId, request.RoleId);
                return result;
            }
        }
    }
}

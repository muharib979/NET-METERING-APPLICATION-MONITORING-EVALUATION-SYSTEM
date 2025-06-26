using Core.Application.Interfaces.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetAllLocation
{
    public class GetPrepaidThanaByDistrictQuery : IRequest<List<DropdownResultForStringKey>>
    {
        public string DistrictCode { get; set; }
        public class Handler : IRequestHandler<GetPrepaidThanaByDistrictQuery, List<DropdownResultForStringKey>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<DropdownResultForStringKey>> Handle(GetPrepaidThanaByDistrictQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetPrepaidThanaByDistrict(request.DistrictCode);
            }
        }
    }
}

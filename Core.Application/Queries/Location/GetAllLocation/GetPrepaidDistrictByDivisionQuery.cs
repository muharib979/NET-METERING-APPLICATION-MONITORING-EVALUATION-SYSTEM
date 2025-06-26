using Core.Application.Interfaces.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetAllLocation
{
    public class GetPrepaidDistrictByDivisionQuery : IRequest<List<DropdownResultForStringKey>>
    {
        public string DivisionCode { get; set; }
        public class Handler : IRequestHandler<GetPrepaidDistrictByDivisionQuery, List<DropdownResultForStringKey>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<DropdownResultForStringKey>> Handle(GetPrepaidDistrictByDivisionQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetPrepaidDistrictByDivision(request.DivisionCode);
            }
        }
    }
}

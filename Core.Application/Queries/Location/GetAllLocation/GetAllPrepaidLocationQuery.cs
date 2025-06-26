using Core.Application.Interfaces.Location;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetAllLocation
{
    public class GetAllPrepaidDivisionQuery :  IRequest<List<DropdownResultForStringKey>>
    {
        public class Handler : IRequestHandler<GetAllPrepaidDivisionQuery, List<DropdownResultForStringKey>>
        {
            private readonly ILocationRepository _repository;
            public Handler(ILocationRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<DropdownResultForStringKey>> Handle(GetAllPrepaidDivisionQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetAllPrepaidDivision();
            }
        }
    }
}

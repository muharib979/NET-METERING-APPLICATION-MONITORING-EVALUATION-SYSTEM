using Core.Application.Interfaces.Religious;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Religious
{
    public class GetLocationByCircleQueryHandler:IRequest<List<LocationsDto>>
    {
        public string CircleCode{ get; set; }

        public class Handler : IRequestHandler<GetLocationByCircleQueryHandler, List<LocationsDto>>
        {
            private readonly IReligiousRepository _repository;
            public Handler(IReligiousRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationsDto>> Handle(GetLocationByCircleQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetLocationByCircle(request.CircleCode);
                return result;
            }
        }
    }
}

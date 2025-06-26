using Core.Application.Interfaces.ZoneCircle;
using Core.Domain.ZoneCircle;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Ministry;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.ZoneCircle.GetAllZoneQueryList
{
    public class GetAllZoneQueryList: IRequest<Response<List<ZoneDto>>>
    {
        public class Handler : IRequestHandler<GetAllZoneQueryList, Response<List<ZoneDto>>>
        {
            private readonly IZoneCircleRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IZoneCircleRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Response<List<ZoneDto>>> Handle(GetAllZoneQueryList request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllZone();
                return Response<List<ZoneDto>>.Success(_mapper.Map<List<ZoneDto>>(result), "Success");
            }
        }
    }
}

using Core.Application.Interfaces.ZoneCircle;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.ZoneCircle.GetAllCircleQueryList
{
    public class GetAllCircleQueryList: IRequest<Response<List<CircleDto>>>
    {
        public class Handler : IRequestHandler<GetAllCircleQueryList, Response<List<CircleDto>>>
        {
            private readonly IZoneCircleRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IZoneCircleRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<List<CircleDto>>> Handle(GetAllCircleQueryList request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllCircle();
                return Response<List<CircleDto>>.Success(_mapper.Map<List<CircleDto>>(result), "Success");
            }
        }
    }
}

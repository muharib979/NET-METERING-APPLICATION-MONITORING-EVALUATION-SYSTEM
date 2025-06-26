using Core.Application.Interfaces.Ministry;
using MediatR;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Ministry;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetAllMinistry
{
    public class GetAllMinistryQueryList: IRequest<Response<List<MinistryDataDto>>>
    {
        public class Handler : IRequestHandler<GetAllMinistryQueryList, Response<List<MinistryDataDto>>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository= ministryRepository;
                _mapper= mapper;
            }
            public async Task<Response<List<MinistryDataDto>>> Handle(GetAllMinistryQueryList request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllMinistry();
                return Response<List<MinistryDataDto>>.Success(_mapper.Map<List<MinistryDataDto>>(result), "Success");

            }
        }
    }
}

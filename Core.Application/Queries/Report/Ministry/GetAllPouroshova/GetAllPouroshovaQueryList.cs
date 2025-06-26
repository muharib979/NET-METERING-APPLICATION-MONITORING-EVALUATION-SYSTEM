using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetAllPouroshova
{
    public class GetAllPouroshovaQueryList : IRequest<List<PouroshovaDTO>>
    {
        public class Handler : IRequestHandler<GetAllPouroshovaQueryList, List<PouroshovaDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }

            public async Task<List<PouroshovaDTO>> Handle(GetAllPouroshovaQueryList request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllPouroshova();
                return result;
            }
        }
    }
}

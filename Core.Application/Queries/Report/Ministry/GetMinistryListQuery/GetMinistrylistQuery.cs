using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetMinistryListQuery
{
    public class GetMinistrylistQuery:IRequest<List<ViewMinistryDTO>>
    {

        public class Handler : IRequestHandler<GetMinistrylistQuery, List<ViewMinistryDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }
            public async Task<List<ViewMinistryDTO>> Handle(GetMinistrylistQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetMinistryList();
                return result;
            }
        }
    }
}

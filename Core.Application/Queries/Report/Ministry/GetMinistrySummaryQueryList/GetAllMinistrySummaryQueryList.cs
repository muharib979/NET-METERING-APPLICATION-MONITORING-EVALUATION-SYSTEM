using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Application.Interfaces.Ministry;
using Core.Application.Queries.Customers.CustomerType;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetMinistrySummaryQueryList
{
    public class GetAllMinistrySummaryQueryList: IRequest<Response<List<MinistrySummaryDto>>>
    {
        public string ReportDate { get; set; }
        public string? LocationCode { get; set; }
        public string ZoneCode { get; set; }

        public class Handler : IRequestHandler<GetAllMinistrySummaryQueryList, Response<List<MinistrySummaryDto>>>
    {
        private readonly IMinistryRepository _repository;
         private readonly IMapper _mapper;

        public Handler(IMinistryRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Response<List<MinistrySummaryDto>>> Handle(GetAllMinistrySummaryQueryList request, CancellationToken cancellationToken)
        {
                var result = await _repository.GetMinistrySummaryByDate(request.ZoneCode,  request.ReportDate,request.LocationCode);

            return Response<List<MinistrySummaryDto>>.Success(_mapper.Map<List<MinistrySummaryDto>>(result), "Success");

        }
    }
}
}

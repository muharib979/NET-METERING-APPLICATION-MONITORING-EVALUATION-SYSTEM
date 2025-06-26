using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetCodeAndDateWiseMinistryDetails
{
    public class GetCodeAndDateWiseMinistryDetailsQueryList: IRequest<Response<List<MinistryDetailsDto>>>
    {
        public string Code { get; set; }
        public bool? IsRebate { get; set; }
        public string BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string? LocationCode { get; set; }
        public string? CircleCode { get; set; }
        public string ReportType { get; set; }

        public class Handler : IRequestHandler<GetCodeAndDateWiseMinistryDetailsQueryList, Response<List<MinistryDetailsDto>>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper) 
            {
                _ministryRepository= ministryRepository;
                _mapper= mapper;
            }
            public async Task<Response<List<MinistryDetailsDto>>> Handle(GetCodeAndDateWiseMinistryDetailsQueryList request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetMinistryAndDateWiseMinistryDetails(request.Code,request.IsRebate, request.BillMonth, request.ZoneCode, request.LocationCode,request.CircleCode, request.ReportType);
                return Response<List<MinistryDetailsDto>>.Success(_mapper.Map<List<MinistryDetailsDto>>(result), "Success");
            }
        }
    }
}

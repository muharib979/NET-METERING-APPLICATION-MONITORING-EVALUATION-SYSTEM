using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetPreModDataListQuery
{
    public class GetPreModDataListQuery: IRequest<List<PreModDataDTO>>
    {
        public string BillMonth { get; set; }
        public string DeptCode { get; set; }
        public class Handler : IRequestHandler<GetPreModDataListQuery, List<PreModDataDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            public Handler(IMinistryRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }

            public async Task<List<PreModDataDTO>> Handle(GetPreModDataListQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetPreModDataByBillMonth(request.BillMonth, request.DeptCode);
                return result;
            }
        }
    }
}

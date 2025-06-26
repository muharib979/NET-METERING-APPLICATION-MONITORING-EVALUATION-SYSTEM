using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetMinistryLedgerByDateQueryList
{
    public class GetMinistryLedgerByDateQuery:IRequest<List<MinistryLedgerDTO>>
    {
        public string BillMonth { get; set; }
        public class Handler : IRequestHandler<GetMinistryLedgerByDateQuery, List<MinistryLedgerDTO>>
        {
            private readonly IMinistryRepository _repository;
         
            public Handler(IMinistryRepository repository)
            {
                _repository = repository;
              
            }

            public async Task<List<MinistryLedgerDTO>> Handle(GetMinistryLedgerByDateQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetMinistryLedgerByDate(request.BillMonth);
                return result;
            }
        }
    }
}

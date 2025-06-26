using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetInitialReadingQuery : IRequest<List<InitialReadingDTO>>
    {
        public string meterTypeCode { get; set; }
        public class Handler : IRequestHandler<GetInitialReadingQuery, List<InitialReadingDTO>>
        {
            private readonly ITemporaryBillRepository _repository;
            public Handler(ITemporaryBillRepository repository)
            {
                _repository=repository;
            }
            public async Task<List<InitialReadingDTO>> Handle(GetInitialReadingQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetInitialReading(request.meterTypeCode);
                return result;

            }
        }
    }
}

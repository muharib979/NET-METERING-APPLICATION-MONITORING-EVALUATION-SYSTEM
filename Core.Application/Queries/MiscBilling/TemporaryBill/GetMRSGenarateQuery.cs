using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetMRSGenarateQuery:IRequest<List<MRSGenarateDTO>>
    {
        public string customerNumber { get; set; }
        public string billCycleCode { get; set; }
        public string locationCode { get; set; }
        public class Handler : IRequestHandler<GetMRSGenarateQuery, List<MRSGenarateDTO>>
        {

            private readonly IMrsGenarateRepository _repository;

            public Handler(IMrsGenarateRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<MRSGenarateDTO>> Handle(GetMRSGenarateQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetMRSGenarate(request.locationCode, request.customerNumber, request.billCycleCode);
                return result;
            }
        }
    }
}

using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllFeederQuery : IRequest<List<FeederDTO>>
    {
        public string locationCode { get; set; }
        public class Handler : IRequestHandler<GetAllFeederQuery, List<FeederDTO>>
        {
            private readonly ITemporaryBillRepository _repository;
            public Handler(ITemporaryBillRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<FeederDTO>> Handle(GetAllFeederQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetAllFeeder(request.locationCode);
                return result;
            }
        }
    }
}

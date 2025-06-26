using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetCustomerCensusListQuery:IRequest<List<CustomerCensusList>>
    {
        public string LocationCode { get; set; }
        public class Handler : IRequestHandler<GetCustomerCensusListQuery, List<CustomerCensusList>>
        {
            private readonly ITemporaryBillRepository _repository;

            public Handler(ITemporaryBillRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<CustomerCensusList>> Handle(GetCustomerCensusListQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetCensusCustomerList(request.LocationCode);
                return result;
            }
        }
    }
}

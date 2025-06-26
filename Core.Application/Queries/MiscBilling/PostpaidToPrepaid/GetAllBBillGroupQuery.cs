using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PostpaidToPrepaid
{
    public class GetAllBBillGroupQuery : IRequest<List<BillGroupDTO>>
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string locationCode { get; set; }
        public class Handler : IRequestHandler<GetAllBBillGroupQuery, List<BillGroupDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<BillGroupDTO>> Handle(GetAllBBillGroupQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllBillGroup(request.locationCode);
                return result;
            }
        }
    }
}


using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.Customers.CustomerType;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.BillingReason
{
    public class GetBillingReasonQuery : IRequest<Response<List<BillReasonDTO>>>
    {
        public class Handler : IRequestHandler<GetBillingReasonQuery, Response<List<BillReasonDTO>>>
        {
            private readonly IBillingReasonRepository _repository;

            public Handler(IBillingReasonRepository repository)
            {
                _repository = repository;
            }

            public async Task<Response<List<BillReasonDTO>>> Handle(GetBillingReasonQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetBillingReason();

                return Response<List<BillReasonDTO>>.Success(result, "Success");

            }
        }
    }
}

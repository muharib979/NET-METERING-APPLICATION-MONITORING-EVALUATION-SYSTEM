using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.Division_District_Thana
{
    public class GetDistrictForPrepaidCustomerQuery : IRequest<List<DistrictDTO>>
    {
        public class Handler : IRequestHandler<GetDistrictForPrepaidCustomerQuery, List<DistrictDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<DistrictDTO>> Handle(GetDistrictForPrepaidCustomerQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetDistrictForPrepaidCustomer();
                return result;
            }
        }
    }
}

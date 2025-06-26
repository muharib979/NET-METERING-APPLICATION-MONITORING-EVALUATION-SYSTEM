using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.Division_District_Thana
{
    public class GetThanaForPrepaidCustomerQuery : IRequest<List<ThanaDTO>>
    {
        public class Handler : IRequestHandler<GetThanaForPrepaidCustomerQuery, List<ThanaDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<ThanaDTO>> Handle(GetThanaForPrepaidCustomerQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetThanaForPrepaidCustomer();
                return result;
            }
        }
    }
}

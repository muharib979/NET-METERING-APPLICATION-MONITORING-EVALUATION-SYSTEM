using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling.PostpaidToPrepaid;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.Division_District_Thana
{
    public class GetDivisionForPrepaidCustomerQuery : IRequest<List<DivisionDTO>>
    {
        public class Handler : IRequestHandler<GetDivisionForPrepaidCustomerQuery, List<DivisionDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<DivisionDTO>> Handle(GetDivisionForPrepaidCustomerQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetDivisionForPrepaidCustomer();
                return result;
            }
        }
    }
}

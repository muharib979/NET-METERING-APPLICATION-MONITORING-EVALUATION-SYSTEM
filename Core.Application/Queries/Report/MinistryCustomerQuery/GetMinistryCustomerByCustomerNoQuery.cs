using Core.Application.Interfaces.MinistryCustomer;
using Shared.DTOs.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetMinistryCustomerByCustomerNoQuery: IRequest<MinistryCustomerDTOs>
    {
        public string CustomerNo { get; set; }
        public class Handler : IRequestHandler<GetMinistryCustomerByCustomerNoQuery, MinistryCustomerDTOs>
        {
            private readonly IMinistryCustomerRepository _repository;
            public Handler(IMinistryCustomerRepository repository)
            {
                _repository = repository;
            }

            public async Task<MinistryCustomerDTOs> Handle(GetMinistryCustomerByCustomerNoQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetMinistryCustomerByCustomerNo(request.CustomerNo);
                return result;
            }
        }
    }
}

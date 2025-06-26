using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Application.Interfaces.Cutomers.ServiceInterfaces;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Customers.CustomerType
{
    public class GetCustomerTypeForBillGenQuery: IRequest<Response<List<CustomerTypeDTO>>>
    {
        public class Handler : IRequestHandler<GetCustomerTypeForBillGenQuery, Response<List< CustomerTypeDTO>>>
        {
            private readonly ICustomerTypeRepository _repository;

            public Handler(ICustomerTypeRepository repository)
            {
                _repository = repository;
            }

            public async Task<Response<List<CustomerTypeDTO>>> Handle(GetCustomerTypeForBillGenQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetCustomerTypeForBillGeneration();

                return Response<List<CustomerTypeDTO>>.Success(result, "Success");

            }
        }
    }
}

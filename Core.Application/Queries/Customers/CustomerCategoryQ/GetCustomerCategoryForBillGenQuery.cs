using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Application.Interfaces.Cutomers.ServiceInterfaces;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Customers.CustomerCategoryQ
{

    public class GetCustomerCategoryForBillGenQuery : IRequest<Response<List<CustomerCategoryDTO>>>
    {
        public class Handler : IRequestHandler<GetCustomerCategoryForBillGenQuery, Response<List<CustomerCategoryDTO>>>
        {
            private readonly ICustomerCategoryRepository _repository;

            public Handler(ICustomerCategoryRepository repository)
            {
                _repository = repository;
            }

            public async Task<Response<List<CustomerCategoryDTO>>> Handle(GetCustomerCategoryForBillGenQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetCustomerCategoryForBillGeneration();
                return Response<List<CustomerCategoryDTO>>.Success(result, "Success");

            }
        }
    }
}

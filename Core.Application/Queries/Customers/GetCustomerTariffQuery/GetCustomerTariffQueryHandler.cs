using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Application.Interfaces.Cutomers.ServiceInterfaces;
using Core.Application.Queries.Customers.CustomerType;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Customers.GetCustomerTariffQuery
{
    public class GetCustomerTariffQueryHandler: IRequest<Response<List<CustomerTariffDto>>>
    {
        public class Handler : IRequestHandler<GetCustomerTariffQueryHandler, Response<List<CustomerTariffDto>>>
         {
                private readonly ICustomerTariffRepository _repository;

                public Handler(ICustomerTariffRepository repository)
                {
                _repository = repository;
                }

                public async Task<Response<List<CustomerTariffDto>>> Handle(GetCustomerTariffQueryHandler request, CancellationToken cancellationToken)
                {

                    var result = await _repository.GetCustomerTariff();

                    return Response<List<CustomerTariffDto>>.Success(result, "Success");

                }
        }
    }
}


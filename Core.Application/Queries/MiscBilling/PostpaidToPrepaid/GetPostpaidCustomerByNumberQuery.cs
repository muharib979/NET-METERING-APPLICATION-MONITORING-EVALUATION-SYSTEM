using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling.Customer_Details;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PostpaidToPrepaid
{
    public class GetPostpaidCustomerByNumberQuery : IRequest<PostpaidCustDetailsDTO>
    {
        public string? customerNumber { get; set; }
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<GetPostpaidCustomerByNumberQuery, PostpaidCustDetailsDTO>
        {
            private readonly IPostpaidCustomerRepository _repository;

            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }

            public async Task<PostpaidCustDetailsDTO> Handle(GetPostpaidCustomerByNumberQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetPostpaidCustomerByNumber(request.customerNumber, request.locationCode);

                return result;

            }
        }
    }
}

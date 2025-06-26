using Core.Application.Interfaces.MiscBilling;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PostpaidToPrepaid
{
    public class GetPrepaidCustomerByTransIdQuery : IRequest<CustomerInformation>
    {
        public string TransID { get; set; }
        public class Handler : IRequestHandler<GetPrepaidCustomerByTransIdQuery, CustomerInformation>
        {
            private readonly IPostpaidCustomerRepository _postpaidCustRepo;
            public Handler(IPostpaidCustomerRepository postpaidCustRepo)
            {
                _postpaidCustRepo = postpaidCustRepo;
            }

            public async Task<CustomerInformation> Handle(GetPrepaidCustomerByTransIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _postpaidCustRepo.GetPrepaidCustomerByTransId(request.TransID);
                return result;
            }

        }
    }
}

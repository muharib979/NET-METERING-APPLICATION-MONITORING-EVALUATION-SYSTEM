using Core.Application.Interfaces.MiscBilling;
using MediatR;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PostpaidToPrepaid
{
    public class GetPostToPrepaidForUpdateQuery: IRequest<Result>
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LocationCode { get; set; }
        public class Handler : IRequestHandler<GetPostToPrepaidForUpdateQuery, Result> 
        {
            private readonly IPostpaidCustomerRepository _postpaidCustRepo;
            public Handler(IPostpaidCustomerRepository postpaidCustRepo)
            {
                _postpaidCustRepo = postpaidCustRepo;
            }

            public async Task<Result> Handle(GetPostToPrepaidForUpdateQuery request, CancellationToken cancellationToken)
            {
                var result = await _postpaidCustRepo.GetPostToPrepaidForUpdate(request.StartDate, request.EndDate, request.LocationCode);
                return result;
            }

        }
    }
}

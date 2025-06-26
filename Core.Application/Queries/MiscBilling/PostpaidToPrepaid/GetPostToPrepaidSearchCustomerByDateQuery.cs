using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PostpaidToPrepaid
{
    public class GetPostToPrepaidSearchCustomerByDateQuery : IRequest<List<PostpaidCustDetailsDTO>>
    {
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<GetPostToPrepaidSearchCustomerByDateQuery, List<PostpaidCustDetailsDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<PostpaidCustDetailsDTO>> Handle(GetPostToPrepaidSearchCustomerByDateQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetPostToPrepaidSearchByDate(request.startDate, request.endDate, request.locationCode);
                return result;
            }
        }
    }
}

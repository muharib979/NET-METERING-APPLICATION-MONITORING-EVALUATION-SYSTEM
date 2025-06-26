using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PostpaidToPrepaid
{
    public class GetPostToPrepaidByDateQuery: IRequest<List<PostToPrepaidViewDTO>>
    {
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<GetPostToPrepaidByDateQuery, List<PostToPrepaidViewDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<PostToPrepaidViewDTO>> Handle(GetPostToPrepaidByDateQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetPostToPrepaidByDate(request.startDate, request.endDate, request.locationCode);
                return result;
            }
        }
    }
}

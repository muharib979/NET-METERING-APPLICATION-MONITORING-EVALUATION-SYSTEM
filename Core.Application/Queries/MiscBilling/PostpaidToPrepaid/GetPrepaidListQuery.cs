using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PostpaidToPrepaid
{
    public class GetPrepaidListQuery : IRequest<List<PostpaidCustDetailsDTO>>
    {
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<GetPrepaidListQuery, List<PostpaidCustDetailsDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<PostpaidCustDetailsDTO>> Handle(GetPrepaidListQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetPrepaidList(request.locationCode);

                return result;

            }
        }
    }
}

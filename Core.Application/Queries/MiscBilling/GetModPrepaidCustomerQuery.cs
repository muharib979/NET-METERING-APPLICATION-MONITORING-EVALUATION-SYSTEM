using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling.PostpaidToPrepaid;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetModPrepaidCustomerQuery : IRequest<List<ModPrepaidCustomerDTO>>
    {
        public string? zoneCode { get; set; }
        public string? locationCode { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public class Handler : IRequestHandler<GetModPrepaidCustomerQuery, List<ModPrepaidCustomerDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<ModPrepaidCustomerDTO>> Handle(GetModPrepaidCustomerQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetModPrepaidCustomerr(request.zoneCode,request.locationCode,request.fromDate,request.toDate);

                return result;
            }
        }
    }
}

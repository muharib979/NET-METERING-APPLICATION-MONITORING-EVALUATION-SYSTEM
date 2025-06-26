using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class LocationWiseCustomerQuery : IRequest<List<LocationWiseCustomerDTO>>
    {
        public string? zoneCode { get; set; }
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<LocationWiseCustomerQuery,List<LocationWiseCustomerDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LocationWiseCustomerDTO>> Handle(LocationWiseCustomerQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.LocationWiseCustomerCount(request.zoneCode, request.locationCode);
                return result;
            }
        }
    }
}

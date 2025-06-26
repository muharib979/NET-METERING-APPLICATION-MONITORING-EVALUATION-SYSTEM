using Core.Application.Interfaces.Ministry;
using Core.Application.Interfaces.MinistryCustomer;
using Core.Application.Queries.Report.Ministry.GetAllUnionPorishod;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetCustomerDataQuery : IRequest<MinistryDataViewDTO>
    {
        public string customerNo { get; set; }
        public string locationCode { get; set; }
        public class Handler : IRequestHandler<GetCustomerDataQuery, MinistryDataViewDTO>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryCustomerRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }

            public async Task<MinistryDataViewDTO> Handle(GetCustomerDataQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetCustomerData(request.customerNo, request.locationCode);
                return result;
            }
        }
    }
}

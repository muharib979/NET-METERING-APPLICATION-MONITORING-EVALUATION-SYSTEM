using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using Core.Application.Interfaces.Ministry;
using Core.Application.Queries.Customers.CustomerType;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetAllMinistryByCurrDateQueryList
{
    public class GetAllMinistryByCurrDateQuery: IRequest<Response<List<MinistryArrearDto>>>
    {
        public string CurrentDate { get; set; }
        public class Handler : IRequestHandler<GetAllMinistryByCurrDateQuery, Response<List<MinistryArrearDto>>>
    {
        private readonly IMinistryRepository _repository;
         private readonly IMapper _mapper;

        public Handler(IMinistryRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Response<List<MinistryArrearDto>>> Handle(GetAllMinistryByCurrDateQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllMinistrybyCurrDate(request.CurrentDate);

            return Response<List<MinistryArrearDto>>.Success(_mapper.Map<List<MinistryArrearDto>>(result), "Success");

        }
    }
}
}

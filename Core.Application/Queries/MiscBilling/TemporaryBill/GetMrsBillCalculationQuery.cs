using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetMrsBillCalculationQuery : IRequest<List<MRSBillCalculationDTO>>
    {
        public string UserLocation { get; set; }
        public string CustomerNumber { get; set; }
        public string BillCycleCode { get; set; }
        public class Handler : IRequestHandler<GetMrsBillCalculationQuery, List<MRSBillCalculationDTO>>
        {
            private readonly IMrsGenarateRepository _repository;

            public Handler(IMrsGenarateRepository repository)
            {
                _repository = repository;
            }

            
            public async Task<List<MRSBillCalculationDTO>> Handle(GetMrsBillCalculationQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetMrsBillCalculation(request.UserLocation, request.CustomerNumber, request.BillCycleCode);
                return result;
            }
        }
    }
}

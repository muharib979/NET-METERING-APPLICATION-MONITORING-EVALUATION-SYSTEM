using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllMeterConditionQuery : IRequest<List<MeterCondionDTO>>
    {
        public class Handler : IRequestHandler<GetAllMeterConditionQuery, List<MeterCondionDTO>>
        {
            private readonly IMeterRepository _repository;
            public Handler(IMeterRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<MeterCondionDTO>> Handle(GetAllMeterConditionQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetAllMeterCondition();
                return result;

            }
        }
    }
}
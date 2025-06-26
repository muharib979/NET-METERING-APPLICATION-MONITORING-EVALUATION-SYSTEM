using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllMeterTypeQuery : IRequest<List<MeterTypeDTO>>
    {
        public class Handler : IRequestHandler<GetAllMeterTypeQuery, List<MeterTypeDTO>>
        {
            private readonly IMeterRepository _repository;
            public Handler(IMeterRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<MeterTypeDTO>> Handle(GetAllMeterTypeQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetAllMeterType();
                return result;

            }
        }
    }
}
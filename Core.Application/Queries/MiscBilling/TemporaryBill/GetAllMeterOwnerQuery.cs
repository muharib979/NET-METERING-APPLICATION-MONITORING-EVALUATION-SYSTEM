using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllMeterOwnerQuery : IRequest<List<MeterOwnerDTO>>
    {
        public class Handler : IRequestHandler<GetAllMeterOwnerQuery, List<MeterOwnerDTO>>
        {
            private readonly IMeterRepository _repository;
            public Handler(IMeterRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<MeterOwnerDTO>> Handle(GetAllMeterOwnerQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetAllMeterOwner();
                return result;

            }
        }
    }
}
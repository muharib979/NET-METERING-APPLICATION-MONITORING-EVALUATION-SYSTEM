using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.MiscCharge
{
    public class GetMiscViewQuery: MiscChargeViewDTO, IRequest<List<MiscChargeViewDTO>>
    {
        public class Handler : IRequestHandler<GetMiscViewQuery, List<MiscChargeViewDTO>>
        {
            private readonly IMiscChargeRepository _repository;

            public Handler(IMiscChargeRepository repository)
            {
                _repository = repository;
            }


           

            public async Task<List<MiscChargeViewDTO>> Handle(GetMiscViewQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetMiscView(request);
                return result;
            }
        }
    }
}

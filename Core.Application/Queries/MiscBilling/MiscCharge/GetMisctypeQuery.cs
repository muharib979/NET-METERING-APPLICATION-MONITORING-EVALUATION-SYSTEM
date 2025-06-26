using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.MiscCharge
{
    public class GetMisctypeQuery: IRequest<Response<List<DcTypeDTO>>>
    {
        public class Handler : IRequestHandler<GetMisctypeQuery, Response<List<DcTypeDTO>>>
        {
            private readonly IMiscChargeRepository _repository;

            public Handler(IMiscChargeRepository repository)
            {
                _repository = repository;
            }

         

            public async Task<Response<List<DcTypeDTO>>> Handle(GetMisctypeQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllDcType();

                return Response<List<DcTypeDTO>>.Success(result, "Successfully Retrived All Dc Type.");
            }
        }
    }
}

using Core.Application.Interfaces.MiscBilling;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetDcTypeQuery : IRequest<Response<List<DcTypeDTO>>>
    {
        public class Handler : IRequestHandler<GetDcTypeQuery, Response<List<DcTypeDTO>>>
        {
            private readonly IDcRcBillGenerateRepository _repository;
            public Handler(IDcRcBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<Response<List<DcTypeDTO>>> Handle(GetDcTypeQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllDcType();

                return Response<List<DcTypeDTO>>.Success(result, "Successfully Retrived All Dc Type.");
            }
        }
    }
}

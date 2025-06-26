using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling.BillingReason;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.Imposed_By
{
    public class GetImposedByQuery : IRequest<Response<List<ImposedByDTO>>>
    {
        public class Handler : IRequestHandler<GetImposedByQuery, Response<List<ImposedByDTO>>>
        {
            private readonly IImposedByRepository _repository;

            public Handler(IImposedByRepository repository)
            {
                _repository = repository;
            }

            public async Task<Response<List<ImposedByDTO>>> Handle(GetImposedByQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetImposedBy();

                return Response<List<ImposedByDTO>>.Success(result, "Success");

            }
        }
    }
}

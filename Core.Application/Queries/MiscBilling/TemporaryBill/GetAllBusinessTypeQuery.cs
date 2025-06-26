using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllBusinessTypeQuery : IRequest<List<BusinessTypeDTO>>
    {
        public class Handler : IRequestHandler<GetAllBusinessTypeQuery, List<BusinessTypeDTO>>
        {
           
            private readonly IBusinessTypeRepository _repository;
            public Handler(IBusinessTypeRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<BusinessTypeDTO>> Handle(GetAllBusinessTypeQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetAllBusinessType();
                return result;

            }
        }
    }
}
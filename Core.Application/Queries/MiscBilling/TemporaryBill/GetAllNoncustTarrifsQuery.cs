using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllNoncustTarrifsQuery:IRequest<List<TariffDTO>>
    {
        public string locationCode { get; set; }

        public class Handler : IRequestHandler<GetAllNoncustTarrifsQuery, List<TariffDTO>>
        {
            private readonly ITarrifRepository _repository;

            public Handler(ITarrifRepository repository)
            {
                _repository = repository;
            }

           
            public async Task<List<TariffDTO>> Handle(GetAllNoncustTarrifsQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllNonCustTariff(request.locationCode); 
                return result;
                   

            }
        }
    }
}

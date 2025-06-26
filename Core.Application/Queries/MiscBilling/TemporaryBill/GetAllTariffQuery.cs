using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllTariffQuery : IRequest<List<TariffDTO>>
    {
        public class Handler : IRequestHandler<GetAllTariffQuery, List<TariffDTO>>
        {
            private readonly ITarrifRepository _repository;
            public Handler(ITarrifRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<TariffDTO>> Handle(GetAllTariffQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetAllTariff();
                return result;

            }
        }
    }
}
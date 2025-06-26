using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllBlockNumQuery: IRequest<List<BlockNumDTO>>
    {

        public string locationCode { get; set; }
        public class Handler:IRequestHandler<GetAllBlockNumQuery, List<BlockNumDTO>>
        {
           
            private readonly IMscLocationRepository _repository;
            public Handler(IMscLocationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<BlockNumDTO>> Handle(GetAllBlockNumQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetBlockNumByLocation(request.locationCode);
                return result;
            }
        }


    }
}

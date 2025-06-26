using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PostpaidToPrepaid
{
    public class GetAllBookNumberQuery : IRequest<List<BlockNumDTO>>
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string locationCode { get; set; }
        public string billgroup { get; set; }
        public class Handler : IRequestHandler<GetAllBookNumberQuery, List<BlockNumDTO>>
        {
            private readonly IPostpaidCustomerRepository _repository;
            public Handler(IPostpaidCustomerRepository repository)
            {
                _repository = repository;
            }
            public async Task<List<BlockNumDTO>> Handle(GetAllBookNumberQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllBookNumber(request.locationCode,request.billgroup);
                return result;
            }
        }
    }
}



using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetAllBillGroupQuery : IRequest<List<BillGroupDTO>>
    {
        public string locationCode { get; set; }
        public string bookNumber { get; set; }
        public class Handler : IRequestHandler<GetAllBillGroupQuery, List<BillGroupDTO>>
        {
           

            private readonly IBookBillGroupRepository _repository;
            public Handler(IBookBillGroupRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<BillGroupDTO>> Handle(GetAllBillGroupQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetAllBillGroup(request.locationCode,request.bookNumber);
                return result;

            }
        }
    }
}
using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.TemporaryBill
{
    public class GetBookNoDDQuery :IRequest<List<BookNoDTO>>
    {
        public string? bookNo { get; set; }
        public string? locationCode { get; set; }
        public string? user { get; set; }
        public class Handler : IRequestHandler<GetBookNoDDQuery, List<BookNoDTO>>
        {
            

            private readonly IBookBillGroupRepository _repository;
            public Handler(IBookBillGroupRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<BookNoDTO>> Handle(GetBookNoDDQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetBookNoAsync(request.bookNo,request.locationCode,request.user);
                return result;

              }
        }
    }
}

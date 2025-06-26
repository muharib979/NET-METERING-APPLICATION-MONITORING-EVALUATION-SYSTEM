using Core.Application.Interfaces.PaymentGateway;
using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.PaymnetGateway.Bkash
{

    public class CheckBillBkashQuery : IRequest<BkashCheckBillResponseDTO>
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string customerNo { get; set; }
        public string? billMonth { get; set; }

        public class Handler : IRequestHandler<CheckBillBkashQuery, BkashCheckBillResponseDTO>
        {
            private readonly IBkashAppRepository _bkashAppRepository;

            public Handler(IBkashAppRepository bkashAppRepository)
            {
                _bkashAppRepository = bkashAppRepository;
            }

            public async Task<BkashCheckBillResponseDTO> Handle(CheckBillBkashQuery query, CancellationToken cancellationToken)
            {
                var result = await _bkashAppRepository.CheckBillBkash(query.userName, query.password, query.customerNo, query.billMonth);
                return result;
            }
        }
    }
}

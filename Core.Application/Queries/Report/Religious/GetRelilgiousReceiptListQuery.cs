using Core.Application.Interfaces.Religious;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Religious
{
    public class GetRelilgiousReceiptListQuery : IRequest<List<ReligiousReceiptDTO>>
    {
        public string LocationCode { get; set; }
        public class Handler : IRequestHandler<GetRelilgiousReceiptListQuery, List<ReligiousReceiptDTO>>
        {
            private readonly IReligiousRepository _religiousRepository;
            public Handler(IReligiousRepository religiousRepository)
            {
                _religiousRepository = religiousRepository;
            }

            

            public async Task<List<ReligiousReceiptDTO>> Handle(GetRelilgiousReceiptListQuery request, CancellationToken cancellationToken)
            {
                var result = await _religiousRepository.GetReligiousReceiptList(request.LocationCode);
                return result;
            }
        }
    }
}

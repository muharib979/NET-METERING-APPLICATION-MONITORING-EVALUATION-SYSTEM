using Core.Application.Interfaces.Religious;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Religious
{
    public class GetAllNoOfMosqueAndPlaceOfWorshipQuery: IRequest<List<NoOfMosqueAndOtherPlaceOfWorshipDTO>>
    {
        public string StartMonth { get; set; }
        public string EndMonth { get; set; }
        public class Handler : IRequestHandler<GetAllNoOfMosqueAndPlaceOfWorshipQuery, List<NoOfMosqueAndOtherPlaceOfWorshipDTO>> 
        {
            private readonly IReligiousRepository _religiousRepository;
            public Handler(IReligiousRepository religiousRepository)
            {
                _religiousRepository= religiousRepository;
            }
            public async Task<List<NoOfMosqueAndOtherPlaceOfWorshipDTO>> Handle(GetAllNoOfMosqueAndPlaceOfWorshipQuery request, CancellationToken cancellationToken)
            {
                var result = await _religiousRepository.GetNoOfMosqueAndWorshipData(request.StartMonth, request.EndMonth);
                return result.ToList();
            }
        }
    }
}

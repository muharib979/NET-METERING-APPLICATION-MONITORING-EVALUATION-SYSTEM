using Core.Application.Interfaces.MISReport;
using Shared.DTOs.MISReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MISReport.ExistingConsumerPenaltyQuery
{
    public class IllegalConsumerPenaltyQuery : IRequest<List<IllegalConsumerPenaltyDTO>>
    {
        //public string ZoneCode { get; set; }
        public string BillMonth { get; set; }


        public class Handler : IRequestHandler<IllegalConsumerPenaltyQuery, List<IllegalConsumerPenaltyDTO>>
        {
            private readonly IMisReportRepository _repository;

            public Handler(IMisReportRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<IllegalConsumerPenaltyDTO>> Handle(IllegalConsumerPenaltyQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetIllegalConsumerPenalty( request.BillMonth);
                return result;
            }
        }
    }
}

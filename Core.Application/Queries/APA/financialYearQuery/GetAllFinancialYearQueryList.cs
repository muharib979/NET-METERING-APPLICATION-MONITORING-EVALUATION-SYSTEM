using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.financialYearQuery
{
    public class GetAllFinancialYearQueryList: IRequest<List<FinancialYearDTO>>
    {
        public class Handler : IRequestHandler<GetAllFinancialYearQueryList, List<FinancialYearDTO>>
        {
            private readonly ITargetRepository _repository;
            public Handler(ITargetRepository programRepository)
            {
                _repository = programRepository;
            }
            public async Task<List<FinancialYearDTO>> Handle(GetAllFinancialYearQueryList request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetFinancialYearList();
                return result;
            }
        }

    }
}

using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.GetIndexUnitQueryList
{
    public class GetIndexUnitQueryList: IRequest<List<IndexUnitDto>>
    {
        public class Handler : IRequestHandler<GetIndexUnitQueryList, List<IndexUnitDto>>
        {
            private readonly IIndexUnitRepository _indexUnitRepository;
            public Handler(IIndexUnitRepository indexUnitRepository)
            {
                _indexUnitRepository = indexUnitRepository;
            }

            public async Task<List<IndexUnitDto>> Handle(GetIndexUnitQueryList request, CancellationToken cancellationToken)
            {
                var result = await _indexUnitRepository.GetIndexUnitDataList();
                return result;
            }
        }
    }
}

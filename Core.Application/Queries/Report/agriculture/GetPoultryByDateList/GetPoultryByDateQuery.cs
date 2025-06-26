using Core.Application.Interfaces.Agriculture.RepositoryInterfaces;
using Shared.DTOs.Agriculture;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.agriculture.GetAgricultureByValidDateList
{
    public class GetPoultryByDateQuery : IRequest<Response<IEnumerable<AgricultureDto>>>
    {
        public string ValidDate { get; set; }

        public class Handler : IRequestHandler<GetPoultryByDateQuery, Response<IEnumerable<AgricultureDto>>>
        {
            private readonly IAgricultureRepository _repository;
            public Handler (IAgricultureRepository repository)
            {
                _repository = repository;
            }
            public async Task<Response<IEnumerable<AgricultureDto>>> Handle(GetPoultryByDateQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllPoultryAsyncByDate(request.ValidDate);

                if (result == null) return Response<IEnumerable<AgricultureDto>>.Fail("No Data Found ");

                return Response<IEnumerable<AgricultureDto>>.Success(result, "Successfully Retrieved");
            }
        }
    }
}

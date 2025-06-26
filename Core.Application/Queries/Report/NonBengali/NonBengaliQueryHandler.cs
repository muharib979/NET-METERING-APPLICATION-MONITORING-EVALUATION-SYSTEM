using Core.Application.Interfaces.NonBengali;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.NonBengali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.NonBengali
{
    public class NonBengaliQueryHandler:IRequest<Response<List<NonBengaliDTOs>>>
    {
        public string validDate { get; set; }
        public string reportType { get; set; }

        public class Handler : IRequestHandler<NonBengaliQueryHandler, Response<List<NonBengaliDTOs>>>
        {
            private readonly INonBengaliRepository _repository;
            public Handler(INonBengaliRepository repository)
            {
                _repository = repository;
            }

            public async Task<Response<List<NonBengaliDTOs>>> Handle(NonBengaliQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetNonBengaliConsumerByDate(request.validDate, request.reportType);
                return Response<List<NonBengaliDTOs>>.Success(result, "Successfully Retrived");
            }
        }
    }
}

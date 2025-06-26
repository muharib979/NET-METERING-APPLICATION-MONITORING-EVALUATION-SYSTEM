using Core.Application.Interfaces.Common.Repository;
using Core.Domain.Report;
using Core.Domain.Temporary;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Report.ReportBillGroupAndBook
{
    public class GetBookByMultipleDBAndLocationCodeQuery : BillGroupDropDownDto, IRequest<Response<List<BookDto>>>
    {


        public class Handler : IRequestHandler<GetBookByMultipleDBAndLocationCodeQuery, Response<List<BookDto>>>
        {
            private readonly ICommonRepository _repository;
            private IMapper _mapper;
            public Handler(ICommonRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<List<BookDto>>> Handle(GetBookByMultipleDBAndLocationCodeQuery request, CancellationToken cancellationToken)
            {
                var result = new List<BookDto>();
                if (request.DbCodes.Length > 0  && (request.LocationCodes == null))
                    result = _mapper.Map<List<BookDto>>(await _repository.getMultipleDatabaseWiseBookData(request.DbCodes, request.BillGroupId));
                else if (request.LocationCodes != null && request.DbCodes.Length > 0)
                        result = _mapper.Map<List<BookDto>>(await _repository.getMultipleLocationWiseBookData(request.DbCodes,request.LocationCodes, request.BillGroupId));
                else
                    if (result == null) return Response<List<BookDto>>.Fail("No Data Found ");

                return Response<List<BookDto>>.Success(result, "Successfully Retrieved");
            }
        }
    }
}

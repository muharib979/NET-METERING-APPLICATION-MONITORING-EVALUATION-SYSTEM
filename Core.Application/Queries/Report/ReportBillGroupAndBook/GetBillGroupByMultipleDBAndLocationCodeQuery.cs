using Core.Application.Interfaces.Common.Repository;
using Core.Domain.Report;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Report.ReportBillGroupAndBook
{
    public class GetBillGroupByMultipleDBAndLocationCodeQuery : BillGroupDropDownDto, IRequest<Response<List<BillGroupDto>>>
    {
      
        public class Handler : IRequestHandler<GetBillGroupByMultipleDBAndLocationCodeQuery, Response<List<BillGroupDto>>>
        {
            private readonly ICommonRepository _repository;
            private IMapper _mapper;
            public Handler(ICommonRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<List<BillGroupDto>>> Handle(GetBillGroupByMultipleDBAndLocationCodeQuery request, CancellationToken cancellationToken)
            {
                var result = new List<BillGroupDto>();
                if (request.DbCodes.Length > 0 && request.LocationCodes.Length==0)
                    result = _mapper.Map<List<BillGroupDto>>(await _repository.getDatabaseWiseBillGroupData(request.DbCodes));
                else if (request.LocationCodes.Length > 0 && request.DbCodes.Length > 0)
                        result = _mapper.Map<List<BillGroupDto>>(await _repository.getLocationWiseBillGroupData(request.DbCodes,request.LocationCodes));
                else
                    if (result == null) return Response<List<BillGroupDto>>.Fail("No Data Found ");

                return Response<List<BillGroupDto>>.Success(result, "Successfully Retrieved");
            }
        }
    }
}

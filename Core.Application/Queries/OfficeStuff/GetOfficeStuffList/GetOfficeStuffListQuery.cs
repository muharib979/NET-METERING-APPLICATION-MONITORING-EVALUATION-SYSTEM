
using Core.Application.Interfaces.OfficeStuff.RepositoryInterface;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.OffiecStuff;

namespace Core.Application.Queries.OfficeStuff.GetOfficeStuffList
{
    public class GetOfficeStuffListQuery : PaginationParams, IRequest<Response<PaginatedList<OfficeStuffDto>>>
    {
        
        public class Handler : IRequestHandler<GetOfficeStuffListQuery, Response<PaginatedList<OfficeStuffDto>>>
        {
            private readonly IOfficeStuffRepository _repository;
            public Handler(IOfficeStuffRepository repository) 
            {
                _repository= repository;
            }
            public async Task<Response<PaginatedList<OfficeStuffDto>>> Handle(GetOfficeStuffListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var officeStuff = await _repository.GetAllAsync(request);
                    if (officeStuff == null)
                    {
                        // _validationMessage.Add("No Office Stuff information found!");
                        return Response<PaginatedList<OfficeStuffDto>>.Fail("No Ofice Stuff information Found");
                    }
                    var paginationList = PaginatedList<OfficeStuffDto>.Create(officeStuff, request.PageNumber, request.pageSize, 10);
                    return Response<PaginatedList<OfficeStuffDto>>.Success(paginationList, "Successfully retirved all office Stuff.");
                }
                catch (Exception ex)
                {
                    // _validationMessage.Add(ex.Message);
                    return Response<PaginatedList<OfficeStuffDto>>.Fail("Failed to retrieve Office Stuff");
                }
            }
        }
    }
}

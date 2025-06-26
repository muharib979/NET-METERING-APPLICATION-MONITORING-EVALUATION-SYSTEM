using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Building.CheckUniqueSiteNbr;

public class CheckUniqueSiteNbrQuery : IRequest<Response<bool>>
{
    public int BuildingId { get; set; }
    public string SiteNbr { get; set; }

    public class Handler : IRequestHandler<CheckUniqueSiteNbrQuery, Response<bool>>
    {
        private readonly IBuildingRepository _repository;
        private readonly IMapper _mapper;
        public Handler(IBuildingRepository repository, IMapper mapper) 
        {
            _repository= repository;
            _mapper= mapper;
        }
        public async Task<Response<bool>> Handle(CheckUniqueSiteNbrQuery request, CancellationToken cancellationToken)
        {
            if (request.BuildingId > 0)
            {
                var current = await _repository.GetByIdAsync(request.BuildingId);
                if (current != null)
                {
                    if (request.SiteNbr == current.SiteNbr) return Response<bool>.Success("Success! SiteNbr is unique unchanged");
                }
            }

            var isUnique = await _repository.IsUniqueSiteNbrAsync(request.SiteNbr);

            if (isUnique == true) return Response<bool>.Fail("SiteNbr already Exists, Please try with another one");

            return Response<bool>.Success(isUnique, "Success! SiteNbr is unique");
        }
    }
}
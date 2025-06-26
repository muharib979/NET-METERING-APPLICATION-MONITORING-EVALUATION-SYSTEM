using Core.Application.Interfaces.Location;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetLocationById 
{
    public class GetLocationByIdHandler : IRequestHandler<GetLocationByIdQuery, Response<LocationsDto>>
    {
        private readonly ILocationRepository _repository;
        private readonly IMapper _mapper;

        public GetLocationByIdHandler(ILocationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

       

        public async Task<Response<LocationsDto>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _repository.GetByIdAsync(request.Id);

            if (location == null) return Response<LocationsDto>.Fail("Failed to Retrieve Building Info");

            return Response<LocationsDto>.Success(location, "Successfully Retrieved Building Info");
        }
    }
}

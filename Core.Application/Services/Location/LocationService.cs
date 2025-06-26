using Core.Application.Interfaces.Location;
using Shared.DTOs.Building;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services.Location
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly IMapper _mapper;
        public LocationService(ILocationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<int> AddAsync(LocationsDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<LocationsDto> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LocationsDto>> GetAllAsync(PaginationParams pParams) => _mapper.Map<List<LocationsDto>>(await _repository.GetAllAsync(pParams));


        public async Task<LocationsDto> GetByIdAsync(int id) => _mapper.Map<LocationsDto>(await _repository.GetByIdAsync(id));
        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(LocationsDto entity)
        {
            throw new NotImplementedException();
        }
    }
}

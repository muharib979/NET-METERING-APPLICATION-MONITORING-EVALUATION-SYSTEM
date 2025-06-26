using Core.Application.Interfaces.Agriculture.RepositoryInterfaces;
using Core.Application.Interfaces.Agriculture.ServiceInterfaces;
using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Shared.DTOs.Agriculture;
using Shared.DTOs.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services.Agriculture
{
    public class AgricultureService : IAgricultureService
    {
        private readonly IAgricultureRepository _repository;
        private readonly IMapper _mapper;
        public AgricultureService(IAgricultureRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<int> AddAsync(AgricultureDto entity) => throw new NotImplementedException();

        public Task<int> AddListAsync(List<AgricultureDto> entity) => throw new NotImplementedException();

        public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

        public Task<AgricultureDto> GetByIdAsync(int id) => throw new NotImplementedException();

        public Task<int> GetTotalCountAsync(string searchBy) => throw new NotImplementedException();

        public Task<int> UpdateAsync(AgricultureDto entity) =>  throw new NotImplementedException();
        

        public async Task<List<AgricultureDto>> GetAllAsync(PaginationParams pParams) => _mapper.Map<List<AgricultureDto>>(await _repository.GetAllAsync(pParams));

        public async Task<IEnumerable<AgricultureDto>> GetAllAsyncByDate(string validDate) => _mapper.Map<IEnumerable<AgricultureDto>>(await _repository.GetAllAsyncByDate(validDate));

        public async Task<IEnumerable<AgricultureDto>> GetAllAgricultureAsyncByDate(string validDate) => _mapper.Map<IEnumerable<AgricultureDto>>(await _repository.GetAllAgricultureAsyncByDate(validDate));

        public async Task<IEnumerable<AgricultureDto>> GetAllPoultryAsyncByDate(string validDate) =>  _mapper.Map<IEnumerable<AgricultureDto>>(await _repository.GetAllPoultryAsyncByDate(validDate));
    }
}

using Core.Application.Interfaces.DatabaseConfig;
using Shared.DTOs.DatabaseConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services.DatabaseConfig
{
    public class DatabaseConfigService : IDatabaseConfigService
    {
        private readonly IDatabaseConfigRepository _repository;
        private readonly IMapper _mapper;
        public DatabaseConfigService(IDatabaseConfigRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<int> AddAsync(DatabaseConfigDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<DatabaseConfigDto> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DatabaseConfigDto>> GetAllAsync(PaginationParams pParams) => _mapper.Map<List<DatabaseConfigDto>>(await _repository.GetAllAsync(pParams));


        public Task<DatabaseConfigDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(DatabaseConfigDto entity)
        {
            throw new NotImplementedException();
        }
    }
}

//using Core.Application.Interfaces.OfficeStuff.RepositoryInterface;
//using Core.Application.Interfaces.OfficeStuff.ServiceInterface;
//using Shared.DTOs.OffiecStuff;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.Application.Services.OfficeStuff
//{
//    public class OfficeStuffService : IOfficeStuffService
//    {
//        private readonly IOfficeStuffRepository _repository;
//        private readonly IMapper _mapper;
//        public OfficeStuffService(IOfficeStuffRepository repository, IMapper mapper)
//        {
//            _repository = repository;
//            _mapper = mapper;
//        }
//        public async Task<int> AddAsync(OfficeStuffDto entity) => await _repository.AddAsync(_mapper.Map<Core.Domain.OfficeStuff.OfficeStuff>(entity));


//        public Task<int> AddListAsync(List<OfficeStuffDto> entity)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<int> DeleteAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<List<OfficeStuffDto>> GetAllAsync(PaginationParams pParams) => _mapper.Map<List<OfficeStuffDto>>(await _repository.GetAllAsync(pParams));
       

//        public Task<OfficeStuffDto> GetByIdAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<int> GetTotalCountAsync(string searchBy)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<int> UpdateAsync(OfficeStuffDto entity)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

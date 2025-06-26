using Core.Application.Commands.ProsoftDataSync.AddProsoftData;
using Core.Application.Interfaces.Building.RepositoryInterfaces;
using Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface;
using Core.Application.Interfaces.ProsoftDataSync.ServiceInterface;
using Core.Domain.ProsoftDataSync;
using Shared.DTOs.Building;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services.ProsoftDataSync
{
    public class ProsoftDataSyncService : IProsoftDataSyncService
    {
        private readonly IProsoftDataSyncRepository _repository;
        private readonly IMapper _mapper;
        public ProsoftDataSyncService(IProsoftDataSyncRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<int> AddAsync(EmployeeDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<EmployeeDTO>> GetAllAsync(PaginationParams pParams)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(EmployeeDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<EmployeeDTO> employeeDTOs)
        {
            foreach (var emp in employeeDTOs)
            {
                string dateTimeStr = emp.Date + " " + emp.ClockTime.Substring(0, 2) + ":" + emp.ClockTime.Substring(2, 2);
                emp.ClockTime = dateTimeStr;
            }
            var employeess = employeeDTOs.Select(x=>_mapper.Map<Employee>(x)).ToList();            
            return _repository.AddListAsync(employeess);
        }
    }
}

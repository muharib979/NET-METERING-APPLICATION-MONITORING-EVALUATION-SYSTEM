using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Common.Interfaces;
using Core.Application.Interfaces.OfficeStuff.RepositoryInterface;
using Core.Domain.OfficeStuff;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.OfficeStuffRepositories
{
    public class OfficeStuffRepository : IOfficeStuffRepository
    {
        private readonly IMapper _mapper;
        
        public OfficeStuffRepository(DapperContext context, IMapper mapper) 
        {
            _mapper= mapper;
        }

        public Task<int> AddAsync(OfficeStuffDto entity)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            var sql = @"INSERT INTO OFFICE_STUFF
                (office_stuff_name, designation, phone, email, is_active)
                VALUES(:OFFICE_STUFF_NAME, :DESIGNATION, :PHONE, :EMAIL, :IS_ACTIVE)";
            return con.ExecuteAsync(sql, entity);
        }

        public Task<int> AddListAsync(List<OfficeStuff> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<OfficeStuffDto> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OfficeStuffDto>> GetAllAsync(PaginationParams pParams)
        {
            List<OfficeStuffDto> officeStuffDtos = new List<OfficeStuffDto>();
            using var con = new OracleConnection(Connection.ConnectionString());
            var sql = $"select * from OFFICE_STUFF";
            var officeStuff = await con.QueryAsync<OfficeStuff>(sql, pParams);
            officeStuffDtos = _mapper.Map(officeStuff.ToList(),officeStuffDtos);
            return officeStuffDtos;
        }

        //public Task<OfficeStuff> GetByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        ////public Task<int> UpdateAsync(OfficeStuff entity)
        ////{
        ////    throw new NotImplementedException();
        //}

        public Task<int> UpdateAsync(OfficeStuffDto entity)
        {
            throw new NotImplementedException();
        }

        Task<OfficeStuffDto> IBaseRepository<OfficeStuffDto>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

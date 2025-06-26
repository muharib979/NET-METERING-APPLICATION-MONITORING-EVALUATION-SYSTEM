using Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface;
using Core.Domain.Building;
using Core.Domain.ProsoftDataSync;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AdminSystem.Infrastructure.Persistence.Repositories.ProsoftDataSyncRepositories
{

    public class ProsoftDataSyncRepository : IProsoftDataSyncRepository
    {
        private readonly IDbConnection _db;
        public ProsoftDataSyncRepository(DapperContext context) => _db = context.GetDbConnection();

        public Task<int> AddAsync(Employee entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddListAsync(List<Employee> entity)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    int inserted = 0;
                    var sql = @"INSERT INTO dbo.prosoft_raw (col_employee_id, col_svc_no, col_rank, col_name, col_clock_type, col_clock_time, col_site_id, col_shift_code, col_deployment, col_entry_time) 
                      VALUES(@col_employee_id, @col_svc_no, @col_rank, @col_name, @col_clock_type, @col_clock_time, @col_site_id, @col_shift_code, @col_deployment, (SELECT now()))";
                    inserted += _db.Execute(sql, entity);
                    transaction.Complete();
                    transaction.Dispose();
                    return await Task.FromResult(inserted);
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    throw;
                }
            }            
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Employee>> GetAllAsync(PaginationParams pParams)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}

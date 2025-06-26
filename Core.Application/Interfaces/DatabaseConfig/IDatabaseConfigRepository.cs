namespace Core.Application.Interfaces.DatabaseConfig
{
    public interface IDatabaseConfigRepository : IBaseRepository<Core.Domain.DatabaseConfig.DatabaseConfig>
    {
        Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetAllDbConfigDDList();
        Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetAllDbConfigListWithIsJob(int isJob = 1);
        Task<Core.Domain.DatabaseConfig.DatabaseConfig> GetDatabaseDataByDbCodeAsync(string dbcode);
        Task<Core.Domain.DatabaseConfig.DatabaseConfig> GetDatabaseInsertByDbCodeAsync(string dbcode);
        Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDatabaseByZoneCode(string zonecode);
        Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDatabaseCodeByZoneCode(string zonecode);
        Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetDatabaseByUserIdRoleId(int userId,int roleId);
        Task<List<DropdownResultForStringKey>> GetDbByCircleCode(string circleCode);
        Task<DropdownResultForStringKey> GetDbByLocationCode(string locCode);
        Task<Core.Domain.DatabaseConfig.DatabaseConfig> GetDatabaseDataBylocCodeAsync(string locationCode);
        Task<List<Core.Domain.DatabaseConfig.DatabaseConfig>> GetAllDbConfigListForFDMPrepaid();
    }
}

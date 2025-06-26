using AutoMapper;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.ReplicationStatus;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.ReplicationStatus;

namespace AdminSystem.Infrastructure.Persistence.Repositories.ReplicationStatus
{
    public class ReplicationStatusRepository : IReplicationStatusRepository
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;

        public ReplicationStatusRepository(IMapper mapper, IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo)
        {
            _mapper = mapper;
            _dbConfigRepo = dbConfigRepo;
            _commRepo = commRepo;
        }

        #region Method
        /// <summary>
        /// Gets a list of replication status details.
        /// </summary>
        /// <param name="billMonth">string billMonth is a code for bill cycle in the format "YYYYMM"</param>
        /// <returns>List<ReplicationStatusDto> of replication status details</returns>
        public async Task<List<Shared.DTOs.ReplicationStatus.ReplicationStatus>> GetAllReplicationStatusList(string billMonth)
        {
            try
            {
                List<Core.Domain.DatabaseConfig.DatabaseConfig> activeDatabases = await _dbConfigRepo.GetAllDbConfigListWithIsJob(0); // 0 is false // Getting databases that are currently active.

                List<Shared.DTOs.ReplicationStatus.ReplicationStatus> replicationStatusInfoList = new List<Shared.DTOs.ReplicationStatus.ReplicationStatus>(activeDatabases.Count());

                for (int i = 0; i < activeDatabases.Count; i++)
                {
                    // Looping through each database to find replication status.
                    string connectionString = _commRepo.CreateConnectionString(activeDatabases[i].HOST, activeDatabases[i].PORT, activeDatabases[i].SERVICE_NAME, activeDatabases[i].USER_ID, activeDatabases[i].PASSWORD); // Getting connection string by database.

                    try
                    {
                        using IDbConnection conn = new OracleConnection(connectionString); // Creating new connection with current connection string.


                        string sQuery = @"select max(CARD_GEN_DATE) AS CARD_GEN_DATE,max(MRS_ENTRY_DATE) MRS_ENTRY_DATE, max(OVERALL_PROC_DATE) OVERALL_PROC_DATE, max(OVERALL_FINAL_DATE) OVERALL_FINAL_DATE, max(BILL_GEN_DATE) BILL_GEN_DATE,  max(BILL_FINAL_DATE) BILL_FINAL_DATE,max(BILL_DESPATCH_DATE) BILL_DESPATCH_DATE, 'Up' as DatabaseStatus, '' as Database, '#a9ffcf' as Color 
                                    from ebc.bc_customer_event_log where bill_cycle_code = :BillCycleCode";

                        var result = await conn.QueryAsync<Shared.DTOs.ReplicationStatus.ReplicationStatus>(sQuery, new
                        {
                            BillCycleCode = billMonth
                        });

                        replicationStatusInfoList.Add(result.FirstOrDefault());
                        replicationStatusInfoList[i].DATABASE = activeDatabases[i].NAME; // Setting value of active database name.

                    }
                    catch (Exception)
                    {
                        Shared.DTOs.ReplicationStatus.ReplicationStatus result = new Shared.DTOs.ReplicationStatus.ReplicationStatus(); // Creating empty ReplicationStatusDetailsDto

                        // Assigning values because database is down as it could not connect.
                        result.DATABASE = activeDatabases[i].NAME; // Setting value of active database name.
                        result.DATABASESTATUS = "Down"; // Setting value of database status to down as database could not connect.
                        result.COLOR = "#ff7f7f";
                        replicationStatusInfoList.Add(result);
                        continue;
                    }
                }
                return replicationStatusInfoList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

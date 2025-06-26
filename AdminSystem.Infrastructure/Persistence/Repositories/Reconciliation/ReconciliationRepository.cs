using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Reconciliation;
using Core.Domain.Reconciliation;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Reconciliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Reconciliation
{
    public class ReconciliationRepository : IReconciliation
    {
        private readonly IMapper _mapper;

        public ReconciliationRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

      public async Task<List<ReconcilationStatusDTO>> GetConsumerReconcilation(string startDate, string endDate,string user)
        {
            try
            {
                List<ReconcilationStatusDTO> reconciliations = new List<ReconcilationStatusDTO>();
                using var con = new OracleConnection(Connection.ConnectionStringConsumerBill());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_FROM_PAY_DATE",value: startDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TO_PAY_DATE", value: endDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: user, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<ReconcilationStatus>(" DPG_PAYBILL_PAYMENT_RECONCILE.DPD_RECECONCILE_DATA", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                reconciliations = _mapper.Map(result, reconciliations);
                return reconciliations;
                

            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        
    

        public async Task<bool> SaveConsumerReconciliation(ReconcilationStatusDTO model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionStringConsumerBill());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_PAY_DATE", value: model.PayDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: model.User, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_PAYBILL_PAYMENT_RECONCILE.DPD_RECECONCILE_SUBMIT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status")>0;
            return isSuccess;
        }


        public async Task<List<ReconcilationStatusDTO>> GetMiscReconcilation(string startDate, string endDate, string user)
        {
            try
            {
                
                List<ReconcilationStatusDTO> reconciliations = new List<ReconcilationStatusDTO>();
                using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                oracleDynamicParameters.Add("P_FROM_PAY_DATE", value: startDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_TO_PAY_DATE", value: endDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                oracleDynamicParameters.Add("P_USER", value: user, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                var result = con.Query<ReconcilationStatus>(" DPG_PAYBILL_PAYMENT_RECONCILE.DPD_RECECONCILE_DATA", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                reconciliations = _mapper.Map(result, reconciliations);
                return reconciliations;


            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        public async Task<bool> SaveMiscReconciliation(ReconcilationStatusDTO model)
        {
            bool isSuccess = true;
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_PAY_DATE", value: model.PayDate, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: model.User, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_PAYBILL_PAYMENT_RECONCILE.DPD_RECECONCILE_SUBMIT", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            return isSuccess;
        }
    }
}

using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class InstallmentPlanRepository : IInstallmentPlanRepository<InstallmentPlanDTO>
    {
        private readonly IMapper _mapper;

        public InstallmentPlanRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<InstallmentPlanDTO>> GetPenaltyBillInstallmentPlan(string billNumber)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output); 
            oracleDynamicParameters.Add("P_BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<InstallmentPlanDTO>("DPG_MISCBILL_PENALTY_BILL_GEN.DPD_CUST_DETAILS_INSTALL", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<List<InstallmentPlanDTO>> GetPenaltyInstallment(string billNumber)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output); //
            oracleDynamicParameters.Add("BILL_NUMBER", value: billNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<InstallmentPlanDTO>("DPG_MISCBILL_INSTALLMENT.DPD_INSTALL_DETAILS", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public async Task<bool> SavePenaltyInstallment(List<InstallmentPlanDTO> model)
        {
            bool isSuccess = true;
            int[] InstallNumberArray = model.Select(model => model.InstallNumber).ToArray();
            string[] principleAmountArray = model.Select(model => model.PrincipleAmount.ToString()).ToArray();
            string[] vatAmountArray = model.Select(model => model.VatAmount.ToString()).ToArray();
            string[] lpsAmountArray = model.Select(model => model.LpsAmount.ToString()).ToArray();
            string[] duedateArray = model.Select(model => model.DueDate.ToString()).ToArray();
            string[] remarkArray = model.Select(model => model.Remarks.ToString()).ToArray();
            string[] userName = model.Select(model => model.UserName.ToString()).ToArray();
            using var con = new OracleConnection(Connection.ConnectionString());

            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_BILL_NO", value: model[0].BillNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_FIRST_INST_PERC", value: model[0].InstallmentPercn, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REST_INST_NO", value: model[0].InstallNumber, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_REMARKS", value: model[0].Remarks, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_Install_No", value: InstallNumberArray, dbType: (OracleMappingType?)OracleDbType.Int32, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            oracleDynamicParameters.Add("P_Prn_Amount", value: principleAmountArray, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            oracleDynamicParameters.Add("P_Vat_Amount", value: vatAmountArray, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            oracleDynamicParameters.Add("P_Lps_Amount", value: lpsAmountArray, dbType: (OracleMappingType?)OracleDbType.Decimal, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            oracleDynamicParameters.Add("P_Due_Date", value: duedateArray, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            oracleDynamicParameters.Add("P_REMARKS_DTL", value: remarkArray, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
            oracleDynamicParameters.Add("P_USER", value: userName, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<int>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_PENALTY_BILL_INSTALL_SAVE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
            isSuccess = oracleDynamicParameters.Get<int>("O_Status") > 0;
            return (isSuccess);
        }
    }
}

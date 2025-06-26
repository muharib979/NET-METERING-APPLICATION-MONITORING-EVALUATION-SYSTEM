using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class VatLpsRepository : IVatLpsRepository
    {
        private readonly IMapper _mapper;

        public VatLpsRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<VatLpsDTO> GetDcRcVatLps(string customerNumber, string locationCode)
        {
            VatLpsDTO vatLps = new VatLpsDTO();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            dynamicParameters.Add("P_CUST_NUM", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER", value: " ", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.QuerySingle<VatLps>("DPG_MISCBILL_DC_RC_BILL_CUST.DPD_VAT_RATE", param: dynamicParameters, commandType: CommandType.StoredProcedure);

            OracleDynamicParameters oDynamicParameters = new OracleDynamicParameters();
            oDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oDynamicParameters.Add("P_USER", value: " ", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var lpsAmount = con.QuerySingle<VatLps>("DPG_MISCBILL_DC_RC_BILL_CUST.DPD_LPS_RATE", param: oDynamicParameters, commandType: CommandType.StoredProcedure);
            result.LPS_RATE = lpsAmount.LPS_RATE;
            vatLps = _mapper.Map(result, vatLps);
            return vatLps;
        }

        public async Task<VatLpsDTO> GetPenaltyVatLps(string customerNumber, string locationCode)
        {
            VatLpsDTO vatLps = new VatLpsDTO();
            VatLps vatLpsEntity = new VatLps();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            dynamicParameters.Add("P_CUST_NUM", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER", value: " ", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            vatLpsEntity = con.QuerySingle<VatLps>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_VAT_RATE", param: dynamicParameters, commandType: CommandType.StoredProcedure);

            OracleDynamicParameters oDynamicParameters = new OracleDynamicParameters();
            oDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oDynamicParameters.Add("P_USER", value: " ", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var lpsAmount = con.QuerySingle<VatLps>("DPG_MISCBILL_PENALTY_BILL_CUST.DPD_LPS_RATE", param: oDynamicParameters, commandType: CommandType.StoredProcedure);
            vatLpsEntity.LPS_RATE = lpsAmount.LPS_RATE;
            vatLps = _mapper.Map(vatLpsEntity, vatLps);
            return vatLps;
        }

        public async Task<VatLpsDTO> GetSupplementaryVatLps(string customerNumber, string locationCode)
        {
            VatLpsDTO vatLps = new VatLpsDTO();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            dynamicParameters.Add("P_CUST_NUM", value: customerNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            dynamicParameters.Add("P_USER", value: " ", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.QuerySingle<VatLps>("DPG_MISCBILL_SUPPLMNTR_BILL.DPD_VAT_RATE", param: dynamicParameters, commandType: CommandType.StoredProcedure);

            OracleDynamicParameters oDynamicParameters = new OracleDynamicParameters();
            oDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oDynamicParameters.Add("P_USER", value: " ", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var lpsAmount = con.QuerySingle<VatLps>("DPG_MISCBILL_SUPPLMNTR_BILL.DPD_LPS_RATE", param: oDynamicParameters, commandType: CommandType.StoredProcedure);

            result.LPS_RATE = lpsAmount.LPS_RATE;

            vatLps = _mapper.Map(result, vatLps);
            return vatLps;
        }
    }
}

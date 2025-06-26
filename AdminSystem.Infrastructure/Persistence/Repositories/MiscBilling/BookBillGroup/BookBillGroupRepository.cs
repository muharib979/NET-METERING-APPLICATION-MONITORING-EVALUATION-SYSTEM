using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Core.Domain.MISCBILL;
using Core.Domain.Temporary;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.BookBillGroup
{
    public class BookBillGroupRepository : IBookBillGroupRepository
    {
        private readonly IMapper _mapper;
        public BookBillGroupRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<BillGroupDTO>> GetAllBillGroup(string locationCode, string bookNumber)
        {
            List<BillGroupDTO> billGroup = new List<BillGroupDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_BOOK_NUM", value: bookNumber, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: locationCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: "", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<BillGroup>("DPG_MISCBILL_NEW_CENSUS.DPD_BILL_GROUP_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            billGroup = _mapper.Map(result, billGroup);
            return billGroup;
        }

        public async Task<List<BookNoDTO>> GetBookNoAsync(string bookno, string locationcode, string user)
        {
            List<BookNoDTO> lstBook = new List<BookNoDTO>();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
            oracleDynamicParameters.Add(":cur_data", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            oracleDynamicParameters.Add("P_BOOK_NUM", value: bookno, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER_LOC_CODE", value: locationcode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            oracleDynamicParameters.Add("P_USER", value: user, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<Books>("DPG_MISCBILL_NEW_CENSUS.DPD_BILL_GROUP_LIST", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            lstBook = _mapper.Map(result, lstBook);
            return lstBook;
        }
    }
}

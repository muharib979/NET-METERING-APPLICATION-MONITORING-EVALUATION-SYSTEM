using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.FileSave
{
    public class FileSaveRepository : IFileSaveRepository
    {
        private readonly IDbConnection _db;
        private readonly IMapper _mapper;

        public FileSaveRepository(DapperContext context, IMapper mapper)
        {
            _db = context.GetDbConnection();
            _mapper = mapper;
        }

        public async Task<string> AddAsync(List<FileSaveDto> model)
        {
            try
            {
               
               using var con = new OracleConnection(Connection.ConnectionString());
                OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();
                foreach (var item in model)
                {
                    oracleDynamicParameters.Add("FILE_NAME", value: item.FileNames[0], dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("FILE_CONTENT", value: item.FileList[0], dbType: (OracleMappingType?)OracleDbType.Blob, direction: ParameterDirection.Input);
                    oracleDynamicParameters.Add("FILE_TYPE", value: item.FileTypes[0], dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    con.Execute("INSERT_FILE", param: oracleDynamicParameters, commandType: CommandType.StoredProcedure);
                }
                    
                
                return "Insert Sucess!";

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        
    }
}

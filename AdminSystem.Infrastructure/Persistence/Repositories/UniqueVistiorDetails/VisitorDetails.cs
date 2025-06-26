using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.VisitorDetails;
using Dapper.Oracle;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using Shared.DTOs.VisitorDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using AutoMapper;
using System.Net;
using Shared.DTOs.CustomerDto;

namespace AdminSystem.Infrastructure.Persistence.Repositories.UniqueVistiorDetails
{
    public class VisitorDetails : IVisitorDetails
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        public VisitorDetails(IHttpContextAccessor httpContext, IMapper mapper) 
        {
            _httpContext = httpContext;
            _mapper = mapper;
        }


        public async Task<List<VisitorCountDTO>> GetVisitorCount()
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            List<VisitorCountDTO> obj = new List<VisitorCountDTO>();
            string sQuery = $@"SELECT COUNT(*) AS VISITORCOUNT FROM MISCBILLAPP_VISITOR_DETAILS";
            var result = await con.QueryAsync<VisitorCountDTO>(sQuery);
            obj = _mapper.Map(result.ToList(), obj);
            return obj;
        }

        //public Task<TokenResponseDtos> NemCheckValidate(TokenRequestDto requestDto)
        //{
        //    var authTokens = new SignInData();
        //    var errorResponse = new TokenResponseDtos
        //    {
        //        Status = 400,
        //        Data = null,
        //        Errors = new List<ErrorDetail>
        //        {
        //            new ErrorDetail
        //            {
        //                Code = "400.1",
        //                Message = "Username is required."
        //            }
        //        }
        //    };

        //    //return Response<TokenResponseDto>.Fail("Username is required");
        //    return Response<TokenResponseDtos>.Successs(authTokens, "Successfully Generated auth token");
        //}

        public async Task<bool> SaveVisitorDetails(string userName)
        {
            bool success = true;
            var ipAddress = _httpContext.HttpContext.Connection.RemoteIpAddress; 
            var forwardedHeader = _httpContext.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault(); 
            if (!string.IsNullOrEmpty(forwardedHeader)) 
            { 
                ipAddress = IPAddress.Parse(forwardedHeader.Split(',').First().Trim()); 
            }
            ipAddress?.ToString();
            using var con = new OracleConnection(Connection.ConnectionString());
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":O_Status", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            param.Add("P_USER_NAME", value: userName, (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_IP_ADDRESS", value: ipAddress, (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = await con.ExecuteAsync("MISC_VISITORDETAILS_PKG.VISITOR_DETAILS_SAVE", param: param, commandType: CommandType.StoredProcedure);
            success = param.Get<int>("O_Status") > 0;
            return success;
        }
    }
}

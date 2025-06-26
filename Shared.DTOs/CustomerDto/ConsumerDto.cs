using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CustomerDto
{
    public  class ConsumerDto
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string MeterNumber { get; set; }
        public string Load { get; set; }
        public string Tariff { get; set; }
        public string OfficeCode { get; set; } 
        public string VoltageLevel { get; set; } 
        public string SiteAddress { get; set; } 
    }
    public class ErrorResponse
    {
        public int Status { get; set; }
        public object Data { get; set; } = new { }; // Default to an empty object
        public List<ErrorDetail> Errors { get; set; }




    }
    public class SignInResponse
    {
        public int Status { get; set; }
        public SignInData Data { get; set; }
        public List<ErrorResponse> Errors { get; set; }
    }
    public class SignInData
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreateTimestamp { get; set; }
    }

    public class ErrorDetail
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class LoginResult
    {
        public bool IsSuccessful { get; set; }
        public string AccessToken { get; set; }
        public List<ErrorDetail> Errors { get; set; }
    }

}

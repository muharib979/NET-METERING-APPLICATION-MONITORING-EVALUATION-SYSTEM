using Shared.DTOs.CustomerDto;
using System.Globalization;
using System.Text.Json;

namespace Core.Application.Common.Exceptions;

public class AppException :Exception
{
    public AppException() : base() { }

    public AppException(string message) : base(message) { }
    public ErrorResponse ErrorResponse { get; set; }

    public AppException(ErrorResponse errorResponse)
        : base(errorResponse.ToString())
    {
        ErrorResponse = errorResponse;
    }
    public override string ToString()
    {
        return JsonSerializer.Serialize(ErrorResponse);
    }


    //public AppException(ErrorResponse errorResponse, params object[] args)
    //  : base(errorResponse.ToString()
    //{
    //    ErrorResponse = errorResponse;
    //}


    //public AppException(string message, params object[] args)
    //    : base(String.Format(CultureInfo.CurrentCulture, message, args))
    //{
    //}
}

using Shared.DTOs.CustomerDto;

namespace Shared.DTOs.Common.Wrappers;

public class ResponseNem<T>
{
    public int StatusCode { get; protected set; }
    public T? Data { get; protected set; }
    public string? Message { get; protected set; }
    //public List<string>? Errors { get; set; }
    public List<ErrorDetail>? Errors { get; set; }

    public static ResponseNem<T> Success(Task<List<CustomerDto.CustomerTypeDTO>> menus, string v)
    {
        var result = new ResponseNem<T>
        {
            StatusCode = 200
        };
        return result;
    }
    public static ResponseNem<T> Success(string message)
    {
        var result = new ResponseNem<T>
        {
            StatusCode = 200,
            Message = message
        };
        return result;
    }

    public static ResponseNem<T> Success(T data, string message)
    {
        var result = new ResponseNem<T>
        {
            StatusCode = 200,
            Data = data,
            Message = message
        };
        return result;
    }

    public static ResponseNem<T> Fail(string message)
    {
        var result = new ResponseNem<T>
        {
            StatusCode = 500,
            Message = message
        };
        return result;
    }





    public static ResponseNem<T> Fail(string message, List<ErrorDetail> errors)
    {
        return new ResponseNem<T>
        {
            StatusCode = 500,
            Message = message,
            Errors = errors
        };
    }


    public static Task<ResponseNem<T>> FailAsync(string message, List<ErrorDetail> errors)
    {
        return Task.FromResult(Fails(message, errors));
    }

    public static ResponseNem<T> Fails(string message, List<ErrorDetail> errors)
    {
        var result = new ResponseNem<T>
        {
            StatusCode = 400,
            //Message = message,
            Data = {},
            Errors = errors
        };
        return result;
    }

    public static ResponseNem<T> Invalid(string message, List<ErrorDetail> errors)
    {
        var result = new ResponseNem<T>
        {
            StatusCode = 401,
            //Message = message,
            Data = {},
            Errors = errors
        };
        return result;
    }



}

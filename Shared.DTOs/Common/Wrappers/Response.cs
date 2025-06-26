using Shared.DTOs.CustomerDto;

namespace Shared.DTOs.Common.Wrappers;

public class Response<T>
{
    public int StatusCode { get; protected set; }
    public T? Data { get; protected set; }
    public string? Message { get; protected set; }
    public List<string>? Errors { get; set; }
    //public List<ErrorDetail>? Errors { get; set; }

    public static Response<T> Success(Task<List<CustomerDto.CustomerTypeDTO>> menus, string v)
    {
        var result = new Response<T>
        {
            StatusCode = 200
        };
        return result;
    }
    public static Response<T> Success(string message)
    {
        var result = new Response<T>
        {
            StatusCode = 200,
            Message = message
        };
        return result;
    }

    public static Response<T> Success(T data, string message)
    {
        var result = new Response<T>
        {
            StatusCode = 200,
            Data = data,
            Message = message
        };
        return result;
    }

    public static Response<T> Successs(T data, string message)
    {
        var result = new Response<T>
        {
            StatusCode = 200,
            Data = data,
            Message = message
        };
        return result;
    }

    public static Response<T> Fail()
    {
        var result = new Response<T>
        {
            StatusCode = 500,
        };
        return result;
    }

    public static Response<T> Fail(string message)
    {
        var result = new Response<T>
        {
            StatusCode = 500,
            Message = message
        };
        return result;
    }

    public static Response<T> Fail(string message, List<string> errors)
    {
        var result = new Response<T>
        {
            StatusCode = 500,
            Message = message,
            Errors = errors
        };
        return result;
    }

    public static Response<T> Fail(List<string> errors)
    {
        var result = new Response<T>
        {
            StatusCode = 500,
            Errors = errors
        };
        return result;
    }



    //public static Response<T> Fails(string message, List<ErrorDetail> errors)
    //{
    //    return new Response<T>
    //    {
    //        StatusCode = 500,
    //        Message = message,
    //        Errors = errors
    //    };
    //}






}

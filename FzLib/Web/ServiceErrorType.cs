using System.Net;

namespace FzLib.Web;

public class ServiceResult
{
    protected ServiceResult()
    {
    }

    public bool IsSuccess { get; init; }
    public string Message { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    public static ServiceResult Success() =>
        new() { IsSuccess = true, StatusCode = HttpStatusCode.OK };

    public static ServiceResult Failure(string msg, HttpStatusCode code) =>
        new() { IsSuccess = false, Message = msg, StatusCode = code };
}

public class ServiceResult<T> : ServiceResult
{
    private ServiceResult()
    {
    }

    public T Value { get; init; }

    public static ServiceResult<T> Success(T value) =>
        new() { IsSuccess = true, StatusCode = HttpStatusCode.OK, Value = value };

    public new static ServiceResult<T> Failure(string msg, HttpStatusCode code) =>
        new() { IsSuccess = false, Message = msg, StatusCode = code };

    public static implicit operator ServiceResult<T>(T value) => Success(value);
}
using System.Net;

namespace FzLib.Web;

public class HttpStatusCodeException : Exception
{
    public HttpStatusCodeException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCodeException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; set; }
}
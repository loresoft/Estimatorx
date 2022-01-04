using System.Net;

namespace EstimatorX.Core;

public class DomainException : Exception
{
    public DomainException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = (int)statusCode;
    }

    public DomainException(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = (int)statusCode;
    }

    public DomainException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public DomainException(int statusCode, string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}

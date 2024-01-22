namespace Infrastructure.Exceptions;

public class HttpException : Exception
{
    public int StatusCode { get; }

    public HttpException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
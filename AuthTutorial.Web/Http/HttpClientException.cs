namespace AuthTutorial.Http;

public class HttpClientException : Exception
{
    public int? ErrorCode { get; }

    public int StatusCode { get; }

    public HttpClientException(int statusCode)
    {
        StatusCode = statusCode;
    }
}
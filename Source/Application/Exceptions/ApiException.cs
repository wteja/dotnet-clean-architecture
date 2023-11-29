using System.Globalization;

namespace Application.Exceptions;

public class ApiException : Exception
{
    public int? StatusCode { get; set; }

    public ApiException() : base()
    {
    }

    public ApiException(string message) : base(message)
    {
    }

    public ApiException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public ApiException(string message, params string[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
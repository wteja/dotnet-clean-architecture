namespace Application.Wrappers;

public class Response<T>
{

    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public ICollection<string>? Errors { get; set; }
    public T? Data { get; set; }

    public Response()
    {
        Succeeded = false;
        Message = null;
        Errors = null;
    }

    public Response(T? data)
    {
        Succeeded = true;
        Data = data;
    }

    public Response(T data, string message)
    {
        Succeeded = true;
        Data = data;
        Message = message;
    }

    public Response(T data, string message, ICollection<string> errors)
    {
        Succeeded = false;
        Data = data;
        Message = message;
        Errors = errors;
    }

    public Response(string message)
    {
        Succeeded = false;
        Message = message;
    }

    public Response(ICollection<string> errors)
    {
        Succeeded = false;
        Errors = errors;
    }
}
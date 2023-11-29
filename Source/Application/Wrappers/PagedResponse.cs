namespace Application.Wrappers;

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PagedResponse() : base()
    {
        
    }

    public PagedResponse(T data, int pageNumber, int pageSize)
    {
        Succeeded = true;
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Message = null;
        Errors = null;
    }
}
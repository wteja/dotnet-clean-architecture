namespace Application.Parameters;

public class RequestParameter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string keyword { get; set; }
    public RequestParameter()
    {
        PageNumber = 1;
        PageSize = 10;
        keyword = "";
    }

    public RequestParameter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize < 1 ? 10 : pageSize;
        keyword = (keyword != null) ? keyword : "";
    }
}
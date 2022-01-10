namespace API.Errors
{
  public class ApiExceptions : ApiResponse
  {
    public ApiExceptions(int statusCode, string message = null, string details = null) : base(statusCode, message)
    {
        Details = details;
    }

    public string Details { get; set; }
  }
}
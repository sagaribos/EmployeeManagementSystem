using EmployeeManagement.Shared.Responses;

namespace EmployeeManagement.Shared.Responses;

public class StandardResponse<T>
{
    public bool IsSuccess { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<ErrorDetails>? Errors { get; set; }
}
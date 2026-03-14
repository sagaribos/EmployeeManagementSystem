using EmployeeManagement.Shared.Responses;

namespace EmployeeManagement.Shared.Helpers;

public static class ResponseHelper
{
    public static StandardResponse<T> Success<T>(T data, string message, string statusCode = "200")
    {
        return new StandardResponse<T>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message,
            Data = data,
            Errors = null
        };
    }

    public static StandardResponse<T> Fail<T>(string message, string statusCode, List<ErrorDetails>? errors = null)
    {
        return new StandardResponse<T>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Data = default,
            Errors = errors
        };
    }
}
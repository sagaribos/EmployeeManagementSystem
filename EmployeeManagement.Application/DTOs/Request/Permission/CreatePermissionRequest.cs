using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Request.Permission;

public class CreatePermissionRequest
{
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public HttpMethodType HttpMethod { get; set; }
}
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Response.Permission;

public class PermissionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public HttpMethodType HttpMethod { get; set; }
    public DateTime CreatedAt { get; set; }
}
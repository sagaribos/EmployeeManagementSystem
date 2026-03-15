using EmployeeManagement.Application.DTOs.Response.Permission;

namespace EmployeeManagement.Application.DTOs.Response.Role;

public class RoleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public IEnumerable<PermissionResponse> Permissions { get; set; }
        = new List<PermissionResponse>();
}
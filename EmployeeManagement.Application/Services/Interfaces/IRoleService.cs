using EmployeeManagement.Application.DTOs.Request.Role;
using EmployeeManagement.Application.DTOs.Response.Role;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync();
    Task<RoleResponse> GetByIdAsync(Guid id);
    Task<RoleResponse> CreateAsync(CreateRoleRequest request);
    Task<RoleResponse> UpdateAsync(Guid id, UpdateRoleRequest request);
    Task DeleteAsync(Guid id);
    Task AssignPermissionAsync(Guid roleId, AssignPermissionRequest request);
    Task RemovePermissionAsync(Guid roleId, Guid permissionId);
}
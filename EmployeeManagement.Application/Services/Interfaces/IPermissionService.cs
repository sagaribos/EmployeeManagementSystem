using EmployeeManagement.Application.DTOs.Request.Permission;
using EmployeeManagement.Application.DTOs.Response.Permission;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionResponse>> GetAllAsync();
    Task<PermissionResponse> GetByIdAsync(Guid id);
    Task<PermissionResponse> CreateAsync(CreatePermissionRequest request);
    Task DeleteAsync(Guid id);
}
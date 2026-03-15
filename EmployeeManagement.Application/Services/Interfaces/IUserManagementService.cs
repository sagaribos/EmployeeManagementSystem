using EmployeeManagement.Application.DTOs.Request.User;
using EmployeeManagement.Application.DTOs.Response.User;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IUserManagementService
{
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<UserResponse> GetByIdAsync(Guid id);
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request);
    Task AssignRoleAsync(Guid userId, AssignRoleRequest request);
    Task RemoveRoleAsync(Guid userId, Guid roleId);
}
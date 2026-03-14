using EmployeeManagement.Application.DTOs.Request.Department;
using EmployeeManagement.Application.DTOs.Response.Department;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentResponse>> GetAllAsync();
    Task<DepartmentResponse> GetByIdAsync(Guid id);
    Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request);
    Task<DepartmentResponse> UpdateAsync(Guid id, UpdateDepartmentRequest request);
    Task DeleteAsync(Guid id);
}
using EmployeeManagement.Application.DTOs.Request.Employee;
using EmployeeManagement.Application.DTOs.Response.Employee;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeResponse>> GetAllAsync();
    Task<EmployeeResponse> GetByIdAsync(Guid id);
    Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request);
    Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request);
    Task DeactivateAsync(Guid id);
}
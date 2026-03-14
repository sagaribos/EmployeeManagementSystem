using EmployeeManagement.Application.DTOs.Request.Designation;
using EmployeeManagement.Application.DTOs.Response.Designation;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IDesignationService
{
    Task<IEnumerable<DesignationResponse>> GetAllAsync();
    Task<DesignationResponse> GetByIdAsync(Guid id);
    Task<DesignationResponse> CreateAsync(CreateDesignationRequest request);
    Task<DesignationResponse> UpdateAsync(Guid id, UpdateDesignationRequest request);
    Task DeleteAsync(Guid id);
}
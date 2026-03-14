using EmployeeManagement.Application.DTOs.Request.Salary;
using EmployeeManagement.Application.DTOs.Response.Salary;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface ISalaryService
{
    Task<SalaryDisbursementResponse> DisburseAsync(SalaryDisbursementRequest request);
    Task<IEnumerable<SalaryDisbursementResponse>> GetByEmployeeIdAsync(Guid employeeId);
    Task<IEnumerable<SalaryDisbursementResponse>> GetByMonthAndYearAsync(int month, int year);
}
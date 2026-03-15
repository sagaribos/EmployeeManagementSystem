using EmployeeManagement.Application.DTOs.Response.Report;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IReportService
{
    Task<IEnumerable<SalarySummaryResponse>> GetMonthlySummaryAsync(int year);
    Task<IEnumerable<DepartmentSalaryResponse>> GetSalaryByDepartmentAsync();
    Task<decimal> GetTotalDisbursedAsync(int month, int year);
    Task<IEnumerable<EmployeeSalaryHistoryResponse>> GetEmployeeSalaryHistoryAsync(Guid employeeId);
}
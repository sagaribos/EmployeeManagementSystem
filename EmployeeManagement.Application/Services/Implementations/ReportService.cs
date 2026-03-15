using EmployeeManagement.Application.DTOs.Response.Report;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SalarySummaryResponse>> GetMonthlySummaryAsync(int year)
    {
        var allSalaries = await _unitOfWork.Salaries.GetAllAsync();

        return allSalaries
            .Where(s => s.Year == year)
            .GroupBy(s => new { s.Month, s.Year })
            .Select(g => new SalarySummaryResponse
            {
                Month = g.Key.Month,
                Year = g.Key.Year,
                TotalDisbursed = g.Sum(s => s.Amount),
                TotalEmployeesPaid = g.Count()
            })
            .OrderBy(s => s.Month)
            .ToList();
    }

    public async Task<IEnumerable<DepartmentSalaryResponse>> GetSalaryByDepartmentAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        var salaries = await _unitOfWork.Salaries.GetAllAsync();

        return employees
            .GroupBy(e => e.Department.Name)
            .Select(g => new DepartmentSalaryResponse
            {
                DepartmentName = g.Key,
                TotalSalary = g.Sum(e => e.BaseSalary),
                TotalEmployees = g.Count()
            })
            .ToList();
    }

    public async Task<decimal> GetTotalDisbursedAsync(int month, int year)
    {
        var salaries = await _unitOfWork.Salaries.GetByMonthAndYearAsync(month, year);
        return salaries.Sum(s => s.Amount);
    }

    public async Task<IEnumerable<EmployeeSalaryHistoryResponse>> GetEmployeeSalaryHistoryAsync(
    Guid employeeId)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
        if (employee is null)
            throw new NotFoundException($"Employee with id {employeeId} not found");

        var salaries = await _unitOfWork.Salaries.GetByEmployeeIdAsync(employeeId);

        return salaries.Select(s => new EmployeeSalaryHistoryResponse
        {
            EmployeeId = employee.Id,
            EmployeeName = employee.FirstName + " " + employee.LastName,
            DepartmentName = employee.Department.Name,
            DesignationTitle = employee.Designation.Title,
            Month = s.Month,
            Year = s.Year,
            Amount = s.Amount,
            Adjustment = s.Adjustment,
            Status = s.Status,
            DisbursedAt = s.DisbursedAt
        }).OrderByDescending(s => s.Year)
          .ThenByDescending(s => s.Month)
          .ToList();
    }

}
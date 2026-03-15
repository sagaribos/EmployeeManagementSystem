using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("monthly-summary/{year}")]
    public async Task<IActionResult> GetMonthlySummary(int year)
    {
        var result = await _reportService.GetMonthlySummaryAsync(year);
        return Ok(ResponseHelper.Success(result, "Monthly summary retrieved successfully"));
    }

    [HttpGet("salary-by-department")]
    public async Task<IActionResult> GetSalaryByDepartment()
    {
        var result = await _reportService.GetSalaryByDepartmentAsync();
        return Ok(ResponseHelper.Success(result, "Department salary report retrieved successfully"));
    }

    [HttpGet("total-disbursed/{month}/{year}")]
    public async Task<IActionResult> GetTotalDisbursed(int month, int year)
    {
        var result = await _reportService.GetTotalDisbursedAsync(month, year);
        return Ok(ResponseHelper.Success(result, "Total disbursed retrieved successfully"));
    }

    [HttpGet("employee-salary-history/{employeeId}")]
    public async Task<IActionResult> GetEmployeeSalaryHistory(Guid employeeId)
    {
        var result = await _reportService.GetEmployeeSalaryHistoryAsync(employeeId);
        return Ok(ResponseHelper.Success(result, "Employee salary history retrieved successfully"));
    }
}
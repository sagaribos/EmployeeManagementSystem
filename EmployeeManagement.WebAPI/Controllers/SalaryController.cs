using EmployeeManagement.Application.DTOs.Request.Salary;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/salary")]
[Authorize]
public class SalaryController : ControllerBase
{
    private readonly ISalaryService _salaryService;

    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService;
    }

    [HttpPost("disburse")]
    public async Task<IActionResult> Disburse([FromBody] SalaryDisbursementRequest request)
    {
        var result = await _salaryService.DisburseAsync(request);
        return Ok(ResponseHelper.Success(result, "Salary disbursed successfully"));
    }

    [HttpGet("history/{employeeId}")]
    public async Task<IActionResult> GetByEmployeeId(Guid employeeId)
    {
        var result = await _salaryService.GetByEmployeeIdAsync(employeeId);
        return Ok(ResponseHelper.Success(result, "Salary history retrieved successfully"));
    }

    [HttpGet("{month}/{year}")]
    public async Task<IActionResult> GetByMonthAndYear(int month, int year)
    {
        var result = await _salaryService.GetByMonthAndYearAsync(month, year);
        return Ok(ResponseHelper.Success(result, "Salary records retrieved successfully"));
    }
}
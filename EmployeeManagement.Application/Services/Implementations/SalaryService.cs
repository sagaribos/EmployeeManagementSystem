using AutoMapper;
using EmployeeManagement.Application.DTOs.Request.Salary;
using EmployeeManagement.Application.DTOs.Response.Salary;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class SalaryService : ISalaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;

    public SalaryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }

    public async Task<SalaryDisbursementResponse> DisburseAsync(SalaryDisbursementRequest request)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(request.EmployeeId);
        if (employee is null)
            throw new NotFoundException($"Employee with id {request.EmployeeId} not found");

        var alreadyDisbursed = await _unitOfWork.Salaries.AlreadyDisbursedAsync(
            request.EmployeeId, request.Month, request.Year);
        if (alreadyDisbursed)
            throw new ConflictException($"Salary already disbursed for this employee for {request.Month}/{request.Year}");

        var totalAmount = employee.BaseSalary + (request.Adjustment ?? 0);

        var salary = new SalaryDisbursement
        {
            EmployeeId = request.EmployeeId,
            Month = request.Month,
            Year = request.Year,
            Amount = totalAmount,
            Adjustment = request.Adjustment,
            Status = SalaryStatus.Paid,
            DisbursedAt = DateTime.UtcNow
        };

        await _unitOfWork.Salaries.AddAsync(salary);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("DISBURSE", "Salary", "System",
            $"Salary disbursed for employee {employee.Email} for {request.Month}/{request.Year}");

        return _mapper.Map<SalaryDisbursementResponse>(salary);
    }

    public async Task<IEnumerable<SalaryDisbursementResponse>> GetByEmployeeIdAsync(Guid employeeId)
    {
        var salaries = await _unitOfWork.Salaries.GetByEmployeeIdAsync(employeeId);
        return _mapper.Map<IEnumerable<SalaryDisbursementResponse>>(salaries);
    }

    public async Task<IEnumerable<SalaryDisbursementResponse>> GetByMonthAndYearAsync(int month, int year)
    {
        var salaries = await _unitOfWork.Salaries.GetByMonthAndYearAsync(month, year);
        return _mapper.Map<IEnumerable<SalaryDisbursementResponse>>(salaries);
    }
}
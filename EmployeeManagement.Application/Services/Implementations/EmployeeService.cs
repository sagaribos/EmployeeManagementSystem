using AutoMapper;
using EmployeeManagement.Application.DTOs.Request.Employee;
using EmployeeManagement.Application.DTOs.Response.Employee;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;

    public EmployeeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<EmployeeResponse>> GetAllAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
    }

    public async Task<EmployeeResponse> GetByIdAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee is null)
            throw new NotFoundException($"Employee with id {id} not found");

        return _mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request)
    {
        var existing = await _unitOfWork.Employees.GetByEmailAsync(request.Email);
        if (existing is not null)
            throw new ConflictException($"Employee with email {request.Email} already exists");

        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
        if (department is null)
            throw new NotFoundException($"Department with id {request.DepartmentId} not found");

        var designation = await _unitOfWork.Designations.GetByIdAsync(request.DesignationId);
        if (designation is null)
            throw new NotFoundException($"Designation with id {request.DesignationId} not found");

        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Gender = request.Gender,
            JoiningDate = request.JoiningDate,
            BaseSalary = request.BaseSalary,
            DepartmentId = request.DepartmentId,
            DesignationId = request.DesignationId,
            Status = EmployeeStatus.Active
        };

        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("CREATE", "Employee", "System", $"Created employee {employee.Email}");

        return _mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee is null)
            throw new NotFoundException($"Employee with id {id} not found");

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Phone = request.Phone;
        employee.Gender = request.Gender;
        employee.BaseSalary = request.BaseSalary;
        employee.DepartmentId = request.DepartmentId;
        employee.DesignationId = request.DesignationId;

        _unitOfWork.Employees.Update(employee);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("UPDATE", "Employee", "System", $"Updated employee {employee.Email}");

        return _mapper.Map<EmployeeResponse>(employee);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee is null)
            throw new NotFoundException($"Employee with id {id} not found");

        employee.Status = EmployeeStatus.Inactive;

        _unitOfWork.Employees.Update(employee);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("DEACTIVATE", "Employee", "System", $"Deactivated employee {employee.Email}");
    }
}
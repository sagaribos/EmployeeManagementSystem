using AutoMapper;
using EmployeeManagement.Application.DTOs.Request.Department;
using EmployeeManagement.Application.DTOs.Response.Department;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;

    public DepartmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<DepartmentResponse>> GetAllAsync()
    {
        var departments = await _unitOfWork.Departments.GetAllAsync();
        return _mapper.Map<IEnumerable<DepartmentResponse>>(departments);
    }

    public async Task<DepartmentResponse> GetByIdAsync(Guid id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department is null)
            throw new NotFoundException($"Department with id {id} not found");

        return _mapper.Map<DepartmentResponse>(department);
    }

    public async Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request)
    {
        var existing = await _unitOfWork.Departments.GetByNameAsync(request.Name);
        if (existing is not null)
            throw new ConflictException($"Department {request.Name} already exists");

        var department = new Department
        {
            Name = request.Name,
            Description = request.Description
        };

        await _unitOfWork.Departments.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("CREATE", "Department", "System", $"Created department {department.Name}");

        return _mapper.Map<DepartmentResponse>(department);
    }

    public async Task<DepartmentResponse> UpdateAsync(Guid id, UpdateDepartmentRequest request)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department is null)
            throw new NotFoundException($"Department with id {id} not found");

        department.Name = request.Name;
        department.Description = request.Description;

        _unitOfWork.Departments.Update(department);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("UPDATE", "Department", "System", $"Updated department {department.Name}");

        return _mapper.Map<DepartmentResponse>(department);
    }

    public async Task DeleteAsync(Guid id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department is null)
            throw new NotFoundException($"Department with id {id} not found");

        _unitOfWork.Departments.Delete(department);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("DELETE", "Department", "System", $"Deleted department {department.Name}");
    }
}
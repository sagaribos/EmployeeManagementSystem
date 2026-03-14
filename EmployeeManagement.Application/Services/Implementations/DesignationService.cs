using AutoMapper;
using EmployeeManagement.Application.DTOs.Request.Designation;
using EmployeeManagement.Application.DTOs.Response.Designation;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class DesignationService : IDesignationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;

    public DesignationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<DesignationResponse>> GetAllAsync()
    {
        var designations = await _unitOfWork.Designations.GetAllAsync();
        return _mapper.Map<IEnumerable<DesignationResponse>>(designations);
    }

    public async Task<DesignationResponse> GetByIdAsync(Guid id)
    {
        var designation = await _unitOfWork.Designations.GetByIdAsync(id);
        if (designation is null)
            throw new NotFoundException($"Designation with id {id} not found");

        return _mapper.Map<DesignationResponse>(designation);
    }

    public async Task<DesignationResponse> CreateAsync(CreateDesignationRequest request)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
        if (department is null)
            throw new NotFoundException($"Department with id {request.DepartmentId} not found");

        var designation = new Designation
        {
            Title = request.Title,
            DepartmentId = request.DepartmentId
        };

        await _unitOfWork.Designations.AddAsync(designation);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("CREATE", "Designation", "System", $"Created designation {designation.Title}");

        return _mapper.Map<DesignationResponse>(designation);
    }

    public async Task<DesignationResponse> UpdateAsync(Guid id, UpdateDesignationRequest request)
    {
        var designation = await _unitOfWork.Designations.GetByIdAsync(id);
        if (designation is null)
            throw new NotFoundException($"Designation with id {id} not found");

        designation.Title = request.Title;
        designation.DepartmentId = request.DepartmentId;

        _unitOfWork.Designations.Update(designation);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("UPDATE", "Designation", "System", $"Updated designation {designation.Title}");

        return _mapper.Map<DesignationResponse>(designation);
    }

    public async Task DeleteAsync(Guid id)
    {
        var designation = await _unitOfWork.Designations.GetByIdAsync(id);
        if (designation is null)
            throw new NotFoundException($"Designation with id {id} not found");

        _unitOfWork.Designations.Delete(designation);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("DELETE", "Designation", "System", $"Deleted designation {designation.Title}");
    }
}
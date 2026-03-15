using AutoMapper;
using EmployeeManagement.Application.DTOs.Request.Permission;
using EmployeeManagement.Application.DTOs.Response.Permission;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Application.Validators;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;

    public PermissionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<PermissionResponse>> GetAllAsync()
    {
        var permissions = await _unitOfWork.Permissions.GetAllAsync();
        return _mapper.Map<IEnumerable<PermissionResponse>>(permissions);
    }

    public async Task<PermissionResponse> GetByIdAsync(Guid id)
    {
        var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
        if (permission is null)
            throw new NotFoundException($"Permission with id {id} not found");

        return _mapper.Map<PermissionResponse>(permission);
    }

    public async Task<PermissionResponse> CreateAsync(CreatePermissionRequest request)
    {
        var validator = new CreatePermissionValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new Shared.Exceptions.ValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToList());

        var existing = await _unitOfWork.Permissions
            .GetByEndpointAndMethodAsync(request.Endpoint, request.HttpMethod.ToString());
        if (existing is not null)
            throw new ConflictException(
                $"Permission for {request.HttpMethod} {request.Endpoint} already exists");

        var permission = new Permission
        {
            Name = request.Name,
            Endpoint = request.Endpoint,
            HttpMethod = request.HttpMethod
        };

        await _unitOfWork.Permissions.AddAsync(permission);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("CREATE", "Permission", "System",
            $"Created permission {permission.Name}");

        return _mapper.Map<PermissionResponse>(permission);
    }

    public async Task DeleteAsync(Guid id)
    {
        var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
        if (permission is null)
            throw new NotFoundException($"Permission with id {id} not found");

        _unitOfWork.Permissions.Delete(permission);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("DELETE", "Permission", "System",
            $"Deleted permission {permission.Name}");
    }
}
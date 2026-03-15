using AutoMapper;
using EmployeeManagement.Application.DTOs.Request.Role;
using EmployeeManagement.Application.DTOs.Response.Role;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Application.Validators;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;

    public RoleService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<RoleResponse>> GetAllAsync()
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleResponse>>(roles);
    }

    public async Task<RoleResponse> GetByIdAsync(Guid id)
    {
        var role = await _unitOfWork.Roles.GetWithPermissionsAsync(id);
        if (role is null)
            throw new NotFoundException($"Role with id {id} not found");

        return _mapper.Map<RoleResponse>(role);
    }

    public async Task<RoleResponse> CreateAsync(CreateRoleRequest request)
    {
        var validator = new CreateRoleValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new Shared.Exceptions.ValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToList());

        var existing = await _unitOfWork.Roles.GetByNameAsync(request.Name);
        if (existing is not null)
            throw new ConflictException($"Role {request.Name} already exists");

        var role = new Role
        {
            Name = request.Name
        };

        await _unitOfWork.Roles.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("CREATE", "Role", "System",
            $"Created role {role.Name}");

        return _mapper.Map<RoleResponse>(role);
    }

    public async Task<RoleResponse> UpdateAsync(Guid id, UpdateRoleRequest request)
    {
        var validator = new UpdateRoleValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new Shared.Exceptions.ValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToList());

        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role is null)
            throw new NotFoundException($"Role with id {id} not found");

        role.Name = request.Name;

        _unitOfWork.Roles.Update(role);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("UPDATE", "Role", "System",
            $"Updated role {role.Name}");

        return _mapper.Map<RoleResponse>(role);
    }

    public async Task DeleteAsync(Guid id)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role is null)
            throw new NotFoundException($"Role with id {id} not found");

        _unitOfWork.Roles.Delete(role);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("DELETE", "Role", "System",
            $"Deleted role {role.Name}");
    }

    public async Task AssignPermissionAsync(Guid roleId, AssignPermissionRequest request)
    {
        var validator = new AssignPermissionValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new Shared.Exceptions.ValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToList());

        var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
        if (role is null)
            throw new NotFoundException($"Role with id {roleId} not found");

        var permission = await _unitOfWork.Permissions.GetByIdAsync(request.PermissionId);
        if (permission is null)
            throw new NotFoundException($"Permission with id {request.PermissionId} not found");

        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = request.PermissionId
        };

        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("ASSIGN", "RolePermission", "System",
            $"Assigned permission {permission.Name} to role {role.Name}");
    }

    public async Task RemovePermissionAsync(Guid roleId, Guid permissionId)
    {
        var role = await _unitOfWork.Roles.GetWithPermissionsAsync(roleId);
        if (role is null)
            throw new NotFoundException($"Role with id {roleId} not found");

        var rolePermission = role.RolePermissions
            .FirstOrDefault(rp => rp.PermissionId == permissionId);

        if (rolePermission is null)
            throw new NotFoundException($"Permission not assigned to this role");

        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("REMOVE", "RolePermission", "System",
            $"Removed permission from role {role.Name}");
    }
}
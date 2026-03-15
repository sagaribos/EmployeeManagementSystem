using AutoMapper;
using EmployeeManagement.Application.DTOs.Request.User;
using EmployeeManagement.Application.DTOs.Response.User;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Application.Validators;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class UserManagementService : IUserManagementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;

    public UserManagementService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        var responses = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _unitOfWork.Users.GetUserRolesAsync(user.Id);
            var response = _mapper.Map<UserResponse>(user);
            response.Roles = roles;
            responses.Add(response);
        }

        return responses;
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user is null)
            throw new NotFoundException($"User with id {id} not found");

        var roles = await _unitOfWork.Users.GetUserRolesAsync(id);
        var response = _mapper.Map<UserResponse>(user);
        response.Roles = roles;

        return response;
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var validator = new CreateUserValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new Shared.Exceptions.ValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToList());

        var existing = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existing is not null)
            throw new ConflictException($"User with email {request.Email} already exists");

        if (request.EmployeeId.HasValue)
        {
            var employee = await _unitOfWork.Employees
                .GetByIdAsync(request.EmployeeId.Value);
            if (employee is null)
                throw new NotFoundException(
                    $"Employee with id {request.EmployeeId} not found");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            EmployeeId = request.EmployeeId
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("CREATE", "User", "System",
            $"Created user {user.Email}");

        var roles = await _unitOfWork.Users.GetUserRolesAsync(user.Id);
        var response = _mapper.Map<UserResponse>(user);
        response.Roles = roles;

        return response;
    }

    public async Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var validator = new UpdateUserValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new Shared.Exceptions.ValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToList());

        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user is null)
            throw new NotFoundException($"User with id {id} not found");

        if (request.EmployeeId.HasValue)
        {
            var employee = await _unitOfWork.Employees
                .GetByIdAsync(request.EmployeeId.Value);
            if (employee is null)
                throw new NotFoundException(
                    $"Employee with id {request.EmployeeId} not found");
        }

        user.Username = request.Username;
        user.Email = request.Email;
        user.EmployeeId = request.EmployeeId;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("UPDATE", "User", "System",
            $"Updated user {user.Email}");

        var roles = await _unitOfWork.Users.GetUserRolesAsync(user.Id);
        var response = _mapper.Map<UserResponse>(user);
        response.Roles = roles;

        return response;
    }

    public async Task AssignRoleAsync(Guid userId, AssignRoleRequest request)
    {
        var validator = new AssignRoleValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new Shared.Exceptions.ValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToList());

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user is null)
            throw new NotFoundException($"User with id {userId} not found");

        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);
        if (role is null)
            throw new NotFoundException($"Role with id {request.RoleId} not found");

        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = request.RoleId
        };

        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("ASSIGN", "UserRole", "System",
            $"Assigned role {role.Name} to user {user.Email}");
    }

    public async Task RemoveRoleAsync(Guid userId, Guid roleId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user is null)
            throw new NotFoundException($"User with id {userId} not found");

        var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
        if (role is null)
            throw new NotFoundException($"Role with id {roleId} not found");

        await _auditLogService.LogAsync("REMOVE", "UserRole", "System",
            $"Removed role {role.Name} from user {user.Email}");
    }
}
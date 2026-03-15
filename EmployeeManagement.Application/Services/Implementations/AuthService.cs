using AutoMapper;
using EmployeeManagement.Application.DTOs.Request.Auth;
using EmployeeManagement.Application.DTOs.Response.Auth;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Application.Validators;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IAuditLogService _auditLogService;

    public AuthService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _auditLogService = auditLogService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        // Add validation
        var validator = new LoginRequestValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new Shared.Exceptions.ValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToList());

        // rest of existing code stays same
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (user is null)
            throw new NotFoundException("Invalid email or password");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new UnauthorizedException("Invalid email or password");

        var roles = await _unitOfWork.Users.GetUserRolesAsync(user.Id);
        var token = _tokenService.GenerateToken(user, roles.ToList());

        await _auditLogService.LogAsync("LOGIN", "User", user.Email, "User logged in");

        return new LoginResponse
        {
            Token = token,
            Email = user.Email,
            Username = user.Username,
            Roles = roles
        };
    }

    public async Task LogoutAsync(string token, string email)
    {
        // Check if already logged out
        var isBlacklisted = await _unitOfWork.Tokens.IsBlacklistedAsync(token);
        if (isBlacklisted)
            throw new UnauthorizedException("You are already logged out");

        // Blacklist the token
        var blacklistedToken = new BlacklistedToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            BlacklistedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };

        await _unitOfWork.Tokens.AddAsync(blacklistedToken);
        await _auditLogService.LogAsync("LOGOUT", "User", email, "User logged out");
    }
}
using EmployeeManagement.Application.DTOs.Request.Auth;
using EmployeeManagement.Application.DTOs.Response.Auth;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task LogoutAsync(string token, string email);
}
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user, IList<string> roles);
}
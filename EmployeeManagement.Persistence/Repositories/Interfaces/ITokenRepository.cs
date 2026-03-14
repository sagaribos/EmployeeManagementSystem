using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Persistence.Repositories.Interfaces;

public interface ITokenRepository
{
    Task AddAsync(BlacklistedToken token);
    Task<bool> IsBlacklistedAsync(string token);
}
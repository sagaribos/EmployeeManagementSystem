using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Persistence.Repositories.Interfaces;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
    Task<Role?> GetWithPermissionsAsync(Guid id);
}
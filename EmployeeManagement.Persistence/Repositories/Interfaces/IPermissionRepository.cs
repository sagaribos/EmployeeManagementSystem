using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Persistence.Repositories.Interfaces;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<Permission?> GetByEndpointAndMethodAsync(string endpoint, string httpMethod);
}
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories.Implementations;

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Permission?> GetByEndpointAndMethodAsync(
        string endpoint, string httpMethod)
        => await _dbSet.FirstOrDefaultAsync(p =>
            p.Endpoint == endpoint &&
            p.HttpMethod.ToString() == httpMethod);
}
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories.Implementations;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
        => await _dbSet.FirstOrDefaultAsync(r => r.Name == name);

    public async Task<Role?> GetWithPermissionsAsync(Guid id)
        => await _dbSet
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == id);
}
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories.Implementations;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Employee?> GetByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(e => e.Email == email);

    public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(Guid departmentId)
        => await _dbSet.Where(e => e.DepartmentId == departmentId).ToListAsync();
}
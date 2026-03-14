using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories.Implementations;

public class DesignationRepository : GenericRepository<Designation>, IDesignationRepository
{
    public DesignationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Designation>> GetByDepartmentIdAsync(Guid departmentId)
        => await _dbSet.Where(d => d.DepartmentId == departmentId).ToListAsync();
}
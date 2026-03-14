using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories.Implementations;

public class SalaryRepository : GenericRepository<SalaryDisbursement>, ISalaryRepository
{
    public SalaryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SalaryDisbursement>> GetByEmployeeIdAsync(Guid employeeId)
        => await _dbSet.Where(s => s.EmployeeId == employeeId).ToListAsync();

    public async Task<IEnumerable<SalaryDisbursement>> GetByMonthAndYearAsync(int month, int year)
        => await _dbSet.Where(s => s.Month == month && s.Year == year).ToListAsync();

    public async Task<bool> AlreadyDisbursedAsync(Guid employeeId, int month, int year)
        => await _dbSet.AnyAsync(s => s.EmployeeId == employeeId && s.Month == month && s.Year == year);
}
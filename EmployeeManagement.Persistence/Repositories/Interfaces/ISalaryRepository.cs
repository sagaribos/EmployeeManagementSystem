using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Persistence.Repositories.Interfaces;

public interface ISalaryRepository : IGenericRepository<SalaryDisbursement>
{
    Task<IEnumerable<SalaryDisbursement>> GetByEmployeeIdAsync(Guid employeeId);
    Task<IEnumerable<SalaryDisbursement>> GetByMonthAndYearAsync(int month, int year);
    Task<bool> AlreadyDisbursedAsync(Guid employeeId, int month, int year);
}
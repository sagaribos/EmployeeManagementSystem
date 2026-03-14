using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Persistence.Repositories.Interfaces;

public interface IDesignationRepository : IGenericRepository<Designation>
{
    Task<IEnumerable<Designation>> GetByDepartmentIdAsync(Guid departmentId);
}
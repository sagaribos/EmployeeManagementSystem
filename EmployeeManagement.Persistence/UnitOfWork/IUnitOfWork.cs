using EmployeeManagement.Persistence.Repositories.Interfaces;

namespace EmployeeManagement.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employees { get; }
    IDepartmentRepository Departments { get; }
    IDesignationRepository Designations { get; }
    ISalaryRepository Salaries { get; }
    IUserRepository Users { get; }
    ITokenRepository Tokens { get; }
    IRoleRepository Roles { get; }               
    IPermissionRepository Permissions { get; }  
    Task<int> SaveChangesAsync();
}
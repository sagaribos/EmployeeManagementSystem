using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Implementations;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using EmployeeManagement.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IEmployeeRepository Employees { get; }
    public IDepartmentRepository Departments { get; }
    public IDesignationRepository Designations { get; }
    public ISalaryRepository Salaries { get; }
    public IUserRepository Users { get; }
    public ITokenRepository Tokens { get; }
    public IRoleRepository Roles { get; }               
    public IPermissionRepository Permissions { get; }   

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Employees = new EmployeeRepository(context);
        Departments = new DepartmentRepository(context);
        Designations = new DesignationRepository(context);
        Salaries = new SalaryRepository(context);
        Users = new UserRepository(context);
        Tokens = new TokenRepository(context);
        Roles = new RoleRepository(context);              
        Permissions = new PermissionRepository(context);   
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}
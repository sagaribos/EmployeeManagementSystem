using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Models;

public class Designation : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }

    // Navigation Properties
    public Department Department { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
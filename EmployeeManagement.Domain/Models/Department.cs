using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Models;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation Properties
    public ICollection<Designation> Designations { get; set; } = new List<Designation>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
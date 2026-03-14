using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Models;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime JoiningDate { get; set; }
    public EmployeeStatus Status { get; set; }
    public decimal BaseSalary { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid DesignationId { get; set; }

    // Navigation Properties
    public Department Department { get; set; } = null!;
    public Designation Designation { get; set; } = null!;
    public ICollection<SalaryDisbursement> SalaryDisbursements { get; set; } = new List<SalaryDisbursement>();
}
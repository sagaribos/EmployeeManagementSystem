using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Request.Employee;

public class UpdateEmployeeRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public decimal BaseSalary { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid DesignationId { get; set; }
}
namespace EmployeeManagement.Application.DTOs.Request.Department;

public class CreateDepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
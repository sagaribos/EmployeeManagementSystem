namespace EmployeeManagement.Application.DTOs.Request.Designation;

public class CreateDesignationRequest
{
    public string Title { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}
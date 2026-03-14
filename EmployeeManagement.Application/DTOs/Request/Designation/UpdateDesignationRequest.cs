namespace EmployeeManagement.Application.DTOs.Request.Designation;

public class UpdateDesignationRequest
{
    public string Title { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}
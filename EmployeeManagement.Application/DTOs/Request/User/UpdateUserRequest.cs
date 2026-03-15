namespace EmployeeManagement.Application.DTOs.Request.User;

public class UpdateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? EmployeeId { get; set; }
}
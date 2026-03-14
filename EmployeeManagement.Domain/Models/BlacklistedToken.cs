namespace EmployeeManagement.Domain.Models;

public class BlacklistedToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime BlacklistedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
namespace EmployeeManagement.Application.Services.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(string action, string entityName, string performedBy, string details);
}
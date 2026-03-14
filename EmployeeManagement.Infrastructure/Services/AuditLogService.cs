using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;

namespace EmployeeManagement.Infrastructure.Services;

public class AuditLogService : IAuditLogService
{
    private readonly LogDbContext _logDbContext;

    public AuditLogService(LogDbContext logDbContext)
    {
        _logDbContext = logDbContext;
    }

    public async Task LogAsync(
        string action,
        string entityName,
        string performedBy,
        string details)
    {
        var log = new AuditLog
        {
            Id = Guid.NewGuid(),
            Action = action,
            EntityName = entityName,
            PerformedBy = performedBy,
            Details = details,
            PerformedAt = DateTime.UtcNow
        };

        await _logDbContext.AuditLogs.AddAsync(log);
        await _logDbContext.SaveChangesAsync();
    }
}
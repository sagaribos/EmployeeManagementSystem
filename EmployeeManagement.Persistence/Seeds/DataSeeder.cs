using EmployeeManagement.Domain.Models;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Seeds;

public static class DataSeeder
{
    // Fixed GUIDs for roles and users so foreign keys always match
    private static readonly Guid AdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid HrRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid ManagerRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly Guid AdminUserId = Guid.Parse("44444444-4444-4444-4444-444444444444");

    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedRolesAsync(context);
        await SeedPermissionsAsync(context);
        await SeedUsersAsync(context);
        await SeedUserRolesAsync(context);
        await SeedRolePermissionsAsync(context);
        await SeedDepartmentsAsync(context);
        await SeedDesignationsAsync(context);
    }

    private static async Task SeedRolesAsync(AppDbContext context)
    {
        if (await context.Roles.AnyAsync()) return;

        var roles = new List<Role>
        {
            new Role
            {
                Id = AdminRoleId,
                Name = "Admin",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Id = HrRoleId,
                Name = "HR",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Id = ManagerRoleId,
                Name = "Manager",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPermissionsAsync(AppDbContext context)
    {
        if (await context.Permissions.AnyAsync()) return;

        var permissions = new List<Permission>
        {
            // Department Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "View Departments",
                Endpoint = "api/departments",
                HttpMethod = HttpMethodType.GET,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Create Department",
                Endpoint = "api/departments",
                HttpMethod = HttpMethodType.POST,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Update Department",
                Endpoint = "api/departments",
                HttpMethod = HttpMethodType.PUT,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Delete Department",
                Endpoint = "api/departments",
                HttpMethod = HttpMethodType.DELETE,
                CreatedAt = DateTime.UtcNow
            },

            // Designation Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "View Designations",
                Endpoint = "api/designations",
                HttpMethod = HttpMethodType.GET,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Create Designation",
                Endpoint = "api/designations",
                HttpMethod = HttpMethodType.POST,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Update Designation",
                Endpoint = "api/designations",
                HttpMethod = HttpMethodType.PUT,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Delete Designation",
                Endpoint = "api/designations",
                HttpMethod = HttpMethodType.DELETE,
                CreatedAt = DateTime.UtcNow
            },

            // Employee Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "View Employees",
                Endpoint = "api/employees",
                HttpMethod = HttpMethodType.GET,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Create Employee",
                Endpoint = "api/employees",
                HttpMethod = HttpMethodType.POST,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Update Employee",
                Endpoint = "api/employees",
                HttpMethod = HttpMethodType.PUT,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Deactivate Employee",
                Endpoint = "api/employees",
                HttpMethod = HttpMethodType.PATCH,
                CreatedAt = DateTime.UtcNow
            },

            // Salary Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Disburse Salary",
                Endpoint = "api/salary",
                HttpMethod = HttpMethodType.POST,
                CreatedAt = DateTime.UtcNow
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "View Salary",
                Endpoint = "api/salary",
                HttpMethod = HttpMethodType.GET,
                CreatedAt = DateTime.UtcNow
            },

            // Report Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Name = "View Reports",
                Endpoint = "api/reports",
                HttpMethod = HttpMethodType.GET,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var adminUser = new User
        {
            Id = AdminUserId,
            Username = "admin",
            Email = "admin@ems.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            CreatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUserRolesAsync(AppDbContext context)
    {
        if (await context.UserRoles.AnyAsync()) return;

        // Verify both user and role exist before inserting
        var userExists = await context.Users.AnyAsync(u => u.Id == AdminUserId);
        var roleExists = await context.Roles.AnyAsync(r => r.Id == AdminRoleId);

        if (!userExists || !roleExists) return;

        var userRole = new UserRole
        {
            UserId = AdminUserId,
            RoleId = AdminRoleId
        };

        await context.UserRoles.AddAsync(userRole);
        await context.SaveChangesAsync();
    }

    private static async Task SeedRolePermissionsAsync(AppDbContext context)
    {
        if (await context.RolePermissions.AnyAsync()) return;

        // Verify admin role exists
        var roleExists = await context.Roles.AnyAsync(r => r.Id == AdminRoleId);
        if (!roleExists) return;

        // Give Admin all permissions
        var allPermissions = await context.Permissions.ToListAsync();
        var adminRolePermissions = allPermissions.Select(p => new RolePermission
        {
            RoleId = AdminRoleId,
            PermissionId = p.Id
        }).ToList();

        await context.RolePermissions.AddRangeAsync(adminRolePermissions);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDepartmentsAsync(AppDbContext context)
    {
        if (await context.Departments.AnyAsync()) return;

        var departments = new List<Department>
        {
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Engineering",
                Description = "Software Engineering Department",
                CreatedAt = DateTime.UtcNow
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Human Resources",
                Description = "HR Department",
                CreatedAt = DateTime.UtcNow
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Finance",
                Description = "Finance Department",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Departments.AddRangeAsync(departments);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDesignationsAsync(AppDbContext context)
    {
        if (await context.Designations.AnyAsync()) return;

        var engineeringDept = await context.Departments
            .FirstOrDefaultAsync(d => d.Name == "Engineering");

        var hrDept = await context.Departments
            .FirstOrDefaultAsync(d => d.Name == "Human Resources");

        if (engineeringDept is null || hrDept is null) return;

        var designations = new List<Designation>
        {
            new Designation
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer",
                DepartmentId = engineeringDept.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Designation
            {
                Id = Guid.NewGuid(),
                Title = "Senior Software Engineer",
                DepartmentId = engineeringDept.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Designation
            {
                Id = Guid.NewGuid(),
                Title = "HR Manager",
                DepartmentId = hrDept.Id,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Designations.AddRangeAsync(designations);
        await context.SaveChangesAsync();
    }
}
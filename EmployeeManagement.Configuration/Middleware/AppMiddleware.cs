using System.Net;
using System.Security.Claims;
using System.Text.Json;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AppValidationException =
    EmployeeManagement.Shared.Exceptions.ValidationException;

namespace EmployeeManagement.Configuration.Middleware;

public class AppMiddleware
{
    private readonly RequestDelegate _next;

    public AppMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
       HttpContext context,
       ITokenRepository tokenRepository,
       IUserRepository userRepository,
       IAuditLogService auditLogService)
    {
        try
        {
            // Step 1 — Check if endpoint is public
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata
                .GetMetadata<IAllowAnonymous>();

            if (allowAnonymous is not null)
            {
                await _next(context);
                return;
            }

            // Step 2 — Check token blacklist
            var token = context.Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var isBlacklisted = await tokenRepository
                    .IsBlacklistedAsync(token);

                if (isBlacklisted)
                {
                    await WriteResponseAsync(
                        context,
                        HttpStatusCode.Unauthorized,
                        "Token has been invalidated. Please login again.");
                    return;
                }
            }

            // Step 3 — Check RBAC permissions
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is not null)
                {
                    var hasPermission = await CheckPermissionAsync(
                        context,
                        userRepository,
                        Guid.Parse(userId));

                    if (!hasPermission)
                    {
                        await WriteResponseAsync(
                            context,
                            HttpStatusCode.Forbidden,
                            "You do not have permission to access this resource.");
                        return;
                    }
                }
            }

            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteResponseAsync(
                context,
                HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteResponseAsync(
                context,
                HttpStatusCode.Conflict,
                ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            await WriteResponseAsync(
                context,
                HttpStatusCode.Unauthorized,
                ex.Message);
        }
        catch (AppValidationException ex)
        {
            await WriteValidationResponseAsync(
                context,
                ex.Errors);
        }
        catch (Exception ex)
        {
            await auditLogService.LogErrorAsync(
                ex.Message,
                ex.StackTrace ?? string.Empty,
                context.Request.Path);

            await WriteResponseAsync(
                context,
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task<bool> CheckPermissionAsync(
        HttpContext context,
        IUserRepository userRepository,
        Guid userId)
    {
        var permissions = await userRepository
            .GetUserPermissionsAsync(userId);

        var requestPath = context.Request.Path.Value?
            .TrimStart('/')
            .ToLower();

        var requestMethod = context.Request.Method.ToUpper();

        return permissions.Any(p =>
            requestPath != null &&
            requestPath.StartsWith(
                p.Endpoint.ToLower().TrimStart('/')) &&
            p.HttpMethod.ToString().ToUpper() == requestMethod);
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new StandardResponse<object>
        {
            IsSuccess = false,
            StatusCode = ((int)statusCode).ToString(),
            Message = message,
            Data = null,
            Errors = null
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    private static async Task WriteValidationResponseAsync(
        HttpContext context,
        List<string> errors)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errorDetails = errors.Select(e => new ErrorDetails
        {
            Field = string.Empty,
            Message = e
        }).ToList();

        var response = new StandardResponse<object>
        {
            IsSuccess = false,
            StatusCode = "400",
            Message = "Validation failed",
            Data = null,
            Errors = errorDetails
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
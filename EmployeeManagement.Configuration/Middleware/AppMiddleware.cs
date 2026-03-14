using System.Net;
using System.Text.Json;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Configuration.Middleware;

public class AppMiddleware
{
    private readonly RequestDelegate _next;

    public AppMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenRepository tokenRepository)
    {
        // Step 1 — Check blacklist unless endpoint is [AllowAnonymous]
        var endpoint = context.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata
            .GetMetadata<IAllowAnonymous>();

        if (allowAnonymous is null)
        {
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
        }

        // Step 2 — Handle all exceptions globally
        try
        {
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
        catch (Shared.Exceptions.ValidationException ex)
        {
            await WriteValidationResponseAsync(
                context,
                ex.Errors);
        }
        catch (Exception ex)
        {
            await WriteResponseAsync(
                context,
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
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
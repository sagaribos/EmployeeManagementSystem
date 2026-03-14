using EmployeeManagement.Persistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Configuration.Middleware;

public class TokenBlacklistMiddleware
{
    private readonly RequestDelegate _next;

    public TokenBlacklistMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITokenRepository tokenRepository)
    {
        var token = context.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            var isBlacklisted = await tokenRepository.IsBlacklistedAsync(token);
            if (isBlacklisted)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new
                {
                    IsSuccess = false,
                    StatusCode = "401",
                    Message = "Token has been invalidated. Please login again.",
                    Data = (object?)null,
                    Errors = (object?)null
                });
                return;
            }
        }

        await _next(context);
    }
}
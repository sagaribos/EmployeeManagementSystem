using System.Net;
using System.Text.Json;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Configuration.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(
                context,
                HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (ConflictException ex)
        {
            await HandleExceptionAsync(
                context,
                HttpStatusCode.Conflict,
                ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            await HandleExceptionAsync(
                context,
                HttpStatusCode.Unauthorized,
                ex.Message);
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            await HandleValidationExceptionAsync(
                context,
                ex.Errors);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    private static async Task HandleExceptionAsync(
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

    private static async Task HandleValidationExceptionAsync(
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
using EmployeeManagement.Application.DTOs.Request.Permission;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/permissions")]
[Authorize]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _permissionService.GetAllAsync();
        return Ok(ResponseHelper.Success(result, "Permissions retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _permissionService.GetByIdAsync(id);
        return Ok(ResponseHelper.Success(result, "Permission retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request)
    {
        var result = await _permissionService.CreateAsync(request);
        return Ok(ResponseHelper.Success(result, "Permission created successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _permissionService.DeleteAsync(id);
        return Ok(ResponseHelper.Success<object>(null!, "Permission deleted successfully"));
    }
}
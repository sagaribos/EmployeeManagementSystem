using EmployeeManagement.Application.DTOs.Request.Designation;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/designations")]
[Authorize]
public class DesignationController : ControllerBase
{
    private readonly IDesignationService _designationService;

    public DesignationController(IDesignationService designationService)
    {
        _designationService = designationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _designationService.GetAllAsync();
        return Ok(ResponseHelper.Success(result, "Designations retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _designationService.GetByIdAsync(id);
        return Ok(ResponseHelper.Success(result, "Designation retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDesignationRequest request)
    {
        var result = await _designationService.CreateAsync(request);
        return Ok(ResponseHelper.Success(result, "Designation created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDesignationRequest request)
    {
        var result = await _designationService.UpdateAsync(id, request);
        return Ok(ResponseHelper.Success(result, "Designation updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _designationService.DeleteAsync(id);
        return Ok(ResponseHelper.Success<object>(null!, "Designation deleted successfully"));
    }
}
using iConductTestTask.Server.Dtos;
using iConductTestTask.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace iConductTestTask.Server.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeResponse>> GetEmployeeWithSubordinates(int id)
    {
        try
        {
            var result = await _service.GetEmployeeWithSubordinates(id);
            EmployeeResponse response = new EmployeeResponse()
            {
                Employee = result
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnable(int id, [FromBody] bool enable)
    {
        try
        {
            await _service.UpdateEmployeeEnable(id, enable);
            return NoContent();
        }
        catch (Exception ex)
        {
             return BadRequest(new { message = ex.Message });
        }
    }
}

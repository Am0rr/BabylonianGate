using BG.App.DTOs;
using BG.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AmmosController : ControllerBase
{
    private IAmmoService _ammoService;

    public AmmosController(IAmmoService ammoService)
    {
        _ammoService = ammoService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAmmoRequest request)
    {
        var id = await _ammoService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ammoService.DeleteAsync(id);
        return Ok(new { message = "Ammo crate deleted successfully." });
    }

    [HttpPut("details")]
    public async Task<IActionResult> UpdateDetails(UpdateAmmoDetailsRequest request)
    {
        await _ammoService.UpdateDetailsAsync(request);
        return Ok(new { message = "Ammo crate details updated successfully." });
    }

    [HttpPost("issue")]
    public async Task<IActionResult> Issue(IssueAmmoRequest request)
    {
        await _ammoService.IssueAmmoAsync(request);
        return Ok(new { message = "Ammo issued successfully." });
    }

    [HttpPost("restock")]
    public async Task<IActionResult> Restock(RestockAmmoRequest request)
    {
        await _ammoService.RestockAsync(request);
        return Ok(new { message = "Ammo restocked successfully." });
    }

    [HttpPost("audit")]
    public async Task<IActionResult> Audit(AuditAmmoInventoryRequest request)
    {
        await _ammoService.AuditInventoryAsync(request);
        return Ok(new { message = "Audit was conducted successfully." });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var crate = await _ammoService.GetCrateByIdAsync(id);

        if (crate is null) return NotFound();

        return Ok(crate);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var crates = await _ammoService.GetAllAsync();

        return Ok(crates);
    }
}
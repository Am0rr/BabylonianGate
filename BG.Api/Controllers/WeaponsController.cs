using BG.App.DTOs;
using BG.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeaponsController : ControllerBase
{
    private readonly IWeaponService _weaponService;

    public WeaponsController(IWeaponService weaponService)
    {
        _weaponService = weaponService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWeaponRequest request)
    {
        var id = await _weaponService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("details")]
    public async Task<IActionResult> UpdateDetails(UpdateWeaponDetailsRequest request)
    {
        await _weaponService.UpdateDetailsAsync(request);
        return Ok(new { message = "Weapon details updated" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _weaponService.DeleteAsync(id);
        return Ok(new { message = "Weapon deleted successfully" });
    }

    [HttpPost("issue")]
    public async Task<IActionResult> Issue(IssueWeaponRequest request)
    {
        await _weaponService.IssueWeaponAsync(request);
        return Ok(new { message = "Weapon issued successfully" });
    }

    [HttpPost("return")]
    public async Task<IActionResult> ReturnToStorage(ReturnWeaponToStorageRequest request)
    {
        await _weaponService.ReturnToStorageAsync(request);
        return Ok(new { message = "Weapon returned successfully" });
    }

    [HttpPost("maintenance")]
    public async Task<IActionResult> SendToMaintenance(SendWeaponToMaintenanceRequest request)
    {
        await _weaponService.SendToMaintenanceAsync(request);
        return Ok(new { message = "Weapon sent to maintenance successfully" });
    }

    [HttpPost("report")]
    public async Task<IActionResult> ReportMissing(ReportWeaponMissingRequest request)
    {
        await _weaponService.ReportMissingAsync(request);
        return Ok(new { message = "Weapon missing reported successfully" });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var weapon = await _weaponService.GetWeaponByIdAsync(id);

        if (weapon is null) return NotFound();

        return Ok(weapon);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var weapons = await _weaponService.GetAllAsync();
        return Ok(weapons);
    }
}
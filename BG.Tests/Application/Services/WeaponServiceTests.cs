using BG.Domain.Interfaces;
using BG.App.Services;
using BG.Domain.Entities;
using BG.Domain.Enums;
using FluentAssertions;
using Moq;
using BG.App.DTOs;

namespace BG.Tests.Application.Services;

public class WeaponServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ISoldierRepository> _soldierRepoMock;
    private readonly Mock<IWeaponRepository> _weaponRepoMock;
    private readonly Mock<ILogRepository> _logRepoMock;

    private readonly WeaponService _service;

    public WeaponServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _soldierRepoMock = new Mock<ISoldierRepository>();
        _weaponRepoMock = new Mock<IWeaponRepository>();
        _logRepoMock = new Mock<ILogRepository>();

        _uowMock.Setup(u => u.Soldiers).Returns(_soldierRepoMock.Object);
        _uowMock.Setup(u => u.Weapons).Returns(_weaponRepoMock.Object);
        _uowMock.Setup(u => u.Logs).Returns(_logRepoMock.Object);

        _service = new WeaponService(_uowMock.Object);
    }

    [Fact]
    public async Task Create_Should_AddWeapon_And_Log()
    {
        var request = new CreateWeaponRequest("AWP", "UK-007", ".338 Lapua", "Sniper");

        var resultId = await _service.CreateAsync(request);

        _weaponRepoMock.Verify(r => r.AddAsync(It.Is<Weapon>(w =>
            w.Codename == "AWP" &&
            w.SerialNumber == "UK-007" &&
            w.Caliber == ".338 Lapua" &&
            w.Type == WeaponType.Sniper
        )), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Create" &&
            l.Details.Contains(request.CodeName) &&
            l.Details.Contains(request.SerialNumber)
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);

        resultId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_Should_RemoveWeapon_And_Log_When_Available()
    {
        var weapon = Weapon.Create("AK-74M", "RU-IZH-9921", "5.45x39mm", WeaponType.Rifle);
        var weaponId = weapon.Id;

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        await _service.DeleteAsync(weaponId);

        _weaponRepoMock.Verify(r => r.Delete(weapon), Times.Once);

        _logRepoMock.Verify(l => l.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Delete" &&
            l.Details.Contains(weapon.Codename) &&
            l.Details.Contains(weapon.SerialNumber)
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_Throw_When_WeaponIsDeployed()
    {
        var weapon = Weapon.Create("Barrett M82A1", "US-BAR-50BMG", ".50 BMG", WeaponType.Sniper);
        var weaponId = weapon.Id;

        var soldierId = Guid.NewGuid();

        weapon.IssueTo(soldierId);

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        await _service.Invoking(s => s.DeleteAsync(weaponId))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot delete weapon because it is currently issued to a soldier. Return it to storage first.");

        _weaponRepoMock.Verify(r => r.Delete(It.IsAny<Weapon>()), Times.Never);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_Should_Throw_When_NotFound()
    {
        var randomId = Guid.NewGuid();

        _weaponRepoMock.Setup(r => r.GetByIdAsync(randomId)).ReturnsAsync((Weapon?)null);

        await _service.Invoking(s => s.DeleteAsync(randomId)).Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Update_Should_ModifyFields_And_Log_When_Changed()
    {
        var weapon = Weapon.Create("Glock 19X", "AU-G19-X001", "9mm", WeaponType.Pistol);
        var weaponId = weapon.Id;

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        var request = new UpdateWeaponDetailsRequest(weaponId, "Glock 19X", "AU-G19-X015", "9mm");

        await _service.UpdateDetailsAsync(request);

        weapon.SerialNumber.Should().Be("AU-G19-X015");

        _weaponRepoMock.Verify(r => r.Update(weapon), Times.Once);

        _logRepoMock.Verify(l => l.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Update"
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_Should_DoNothing_When_NoChanges()
    {
        var weapon = Weapon.Create("HK-416", "DE-HK-556-A5", "5.56x45mm", WeaponType.Rifle);
        var weaponId = weapon.Id;

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        var request = new UpdateWeaponDetailsRequest(weaponId, "HK-416", "DE-HK-556-A5", "5.56x45mm");

        await _service.UpdateDetailsAsync(request);

        _weaponRepoMock.Verify(r => r.Update(It.IsAny<Weapon>()), Times.Never);

        _logRepoMock.Verify(r => r.AddAsync(It.IsAny<OperationLog>()), Times.Never);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Issue_Should_AssignWeaponToSoldier_And_Log()
    {
        var soldier = Soldier.Create("Rick", "Grimes", SoldierRank.Sergeant);
        var weapon = Weapon.Create("AK-47", "SN-479912", "7.62x39mm", WeaponType.Rifle);

        var soldierId = soldier.Id;
        var weaponId = weapon.Id;

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);
        _soldierRepoMock.Setup(r => r.GetByIdAsync(soldierId)).ReturnsAsync(soldier);

        var request = new IssueWeaponRequest(weaponId, soldierId);

        await _service.IssueWeaponAsync(request);

        weapon.IssuedToSoldierId.Should().Be(soldierId);

        weapon.Status.Should().Be(WeaponStatus.Deployed);

        _weaponRepoMock.Verify(r => r.Update(weapon), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Issue" &&
            l.Details.Contains(weapon.Codename) &&
            l.Details.Contains(weapon.SerialNumber) &&
            l.Details.Contains(soldier.FirstName) &&
            l.Details.Contains(soldier.LastName)
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Issue_Should_Throw_When_SoldierNotFound()
    {
        var weapon = Weapon.Create("Sig Sauer M17", "SIG-US-320", "9mm", WeaponType.Pistol);
        var weaponId = weapon.Id;

        var soldierId = Guid.NewGuid();

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        var request = new IssueWeaponRequest(weaponId, soldierId);

        await _service.Invoking(s => s.IssueWeaponAsync(request))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Soldier with ID {request.SoldierId} not found.");

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Issue_Should_Throw_When_WeaponNotFound()
    {
        var soldier = Soldier.Create("Benjamin", "Poindexter", SoldierRank.Sergeant);
        var soldierId = soldier.Id;

        var weaponId = Guid.NewGuid();

        _soldierRepoMock.Setup(r => r.GetByIdAsync(soldierId)).ReturnsAsync(soldier);

        var request = new IssueWeaponRequest(weaponId, soldierId);

        await _service.Invoking(s => s.IssueWeaponAsync(request))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Weapon with ID {request.WeaponId} not found.");

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Return_Should_UnassignWeapon_And_Log()
    {
        var weapon = Weapon.Create("Glock 17", "AUS-88123", "9mm", WeaponType.Pistol);
        var weaponId = weapon.Id;

        var soldierId = Guid.NewGuid();

        weapon.IssueTo(soldierId);

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        var request = new ReturnWeaponToStorageRequest(weaponId, 100);
        await _service.ReturnToStorageAsync(request);

        weapon.IssuedToSoldierId.Should().Be(null);
        weapon.Condition.Should().Be(90);

        _weaponRepoMock.Verify(r => r.Update(weapon), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Return" &&
            l.Details.Contains(weapon.Codename) &&
            l.Details.Contains(weapon.SerialNumber) &&
            l.Details.Contains(weapon.Condition.ToString())
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Return_Should_Throw_When_WeaponNotFound()
    {
        var weaponId = Guid.NewGuid();

        var request = new ReturnWeaponToStorageRequest(weaponId, 100);

        await _service.Invoking(s => s.ReturnToStorageAsync(request))
           .Should().ThrowAsync<KeyNotFoundException>()
           .WithMessage($"Weapon with ID {request.WeaponId} not found.");

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Maintenance_Should_ChangeStatus_And_Log()
    {
        var weapon = Weapon.Create("RPG-7", "RU-2291", "40mm", WeaponType.Launcher);
        var weaponId = weapon.Id;

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        var request = new SendWeaponToMaintenanceRequest(weaponId);

        await _service.SendToMaintenanceAsync(request);

        _weaponRepoMock.Verify(r => r.Update(weapon), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Maintenance" &&
            l.Details.Contains(weapon.Codename) &&
            l.Details.Contains(weapon.Codename)
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Missing_Should_ChangeStatus_And_Log()
    {
        var weapon = Weapon.Create("M203 Launcher", "US-GL-112", "40mm", WeaponType.Launcher);
        var weaponId = weapon.Id;

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        var request = new ReportWeaponMissingRequest(weaponId);

        await _service.ReportMissingAsync(request);

        _weaponRepoMock.Verify(r => r.Update(weapon), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Missing" &&
            l.Details.Contains(weapon.Codename) &&
            l.Details.Contains(weapon.Codename)
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_ReturnDTO_When_Found()
    {
        var weapon = Weapon.Create("AS Val", "RU-SP-001", "9x39mm", WeaponType.Rifle);
        var weaponId = weapon.Id;

        _weaponRepoMock.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

        var result = await _service.GetWeaponByIdAsync(weaponId);

        result.Should().NotBeNull();

        result!.Id.Should().Be(weaponId);
        result.CodeName.Should().Be("AS Val");
        result.SerialNumber.Should().Be("RU-SP-001");
        result.Caliber.Should().Be("9x39mm");
    }

    [Fact]
    public async Task GetById_Should_ReturnNull_When_NotFound()
    {
        var randomId = Guid.NewGuid();

        _weaponRepoMock.Setup(r => r.GetByIdAsync(randomId)).ReturnsAsync((Weapon?)null);

        var result = await _service.GetWeaponByIdAsync(randomId);

        result.Should().BeNull();
    }
}
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
    public async Task Issue_Should_Throw_When_WeaponNotFound()
    {
        var request = new IssueWeaponRequest(Guid.NewGuid(), Guid.NewGuid());

        await _service.Invoking(s => s.IssueWeaponAsync(request))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Weapon with ID {request.WeaponId} not found.*");

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Return_Should_UnassignWeapon_And_Log()
    {
        var soldierId = Guid.NewGuid();

        var weapon = Weapon.Create("Glock 17", "AUS-88123", "9mm", WeaponType.Pistol);
        var weaponId = weapon.Id;

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
            l.Details.Contains(weapon.Codename) &&
            l.Details.Contains(weapon.Condition.ToString())
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }
}
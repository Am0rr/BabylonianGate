using BG.Domain.Interfaces;
using BG.App.Services;
using BG.Domain.Entities;
using BG.Domain.Enums;
using FluentAssertions;
using Moq;
using BG.App.DTOs;
using BG.App.Interfaces;

namespace BG.Tests.Application.Services;

public class AmmoServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IAmmoRepository> _ammoRepoMock;
    private readonly Mock<ISoldierRepository> _soldierRepoMock;
    private readonly Mock<ILogRepository> _logRepoMock;

    private readonly AmmoService _service;

    public AmmoServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _ammoRepoMock = new Mock<IAmmoRepository>();
        _soldierRepoMock = new Mock<ISoldierRepository>();
        _logRepoMock = new Mock<ILogRepository>();

        _uowMock.Setup(u => u.Crates).Returns(_ammoRepoMock.Object);
        _uowMock.Setup(u => u.Soldiers).Returns(_soldierRepoMock.Object);
        _uowMock.Setup(u => u.Logs).Returns(_logRepoMock.Object);

        _service = new AmmoService(_uowMock.Object);
    }

    [Fact]
    public async Task Create_Should_AddCrate_And_Log()
    {
        var request = new CreateAmmoRequest("LC-22-556", "5.56x45mm", 1000, "FMJ");

        var resultId = await _service.CreateAsync(request);

        _ammoRepoMock.Verify(r => r.AddAsync(It.Is<AmmoCrate>(a =>
            a.LotNumber == "LC-22-556" &&
            a.Caliber == "5.56x45mm" &&
            a.Quantity == 1000 &&
            a.Type == AmmoType.FMJ
        )), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Create" &&
            l.Details.Contains(request.LotNumber) &&
            l.Details.Contains(request.Caliber) &&
            l.Details.Contains(request.Type)
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);

        resultId.Should().NotBeEmpty();
    }


    [Fact]
    public async Task Delete_Should_RemoveCrate_And_Log_When_QuantityIsZero()
    {
        var crate = AmmoCrate.Create("BLACK-TIP-308", "7.62x51mm", 0, AmmoType.AP);
        var crateId = crate.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);

        await _service.DeleteAsync(crateId);

        _ammoRepoMock.Verify(r => r.Delete(crate), Times.Once);

        _logRepoMock.Verify(l => l.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Delete" &&
            l.Details.Contains(crate.LotNumber) &&
            l.Details.Contains(crate.Caliber)
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_Throw_When_CrateHasAmmo()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);
        var crateId = crate.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);

        await _service.Invoking(s => s.DeleteAsync(crateId)).Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Delete_Should_Throw_When_NotFound()
    {
        var randomId = Guid.NewGuid();

        _ammoRepoMock.Setup(r => r.GetByIdAsync(randomId)).ReturnsAsync((AmmoCrate?)null);

        await _service.Invoking(s => s.DeleteAsync(randomId)).Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Update_Should_UpdateFields_And_Log_When_Changed()
    {
        var crate = AmmoCrate.Create("RED-TRACE-762", "7.62x39mm", 800, AmmoType.Tracer);
        var crateId = crate.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);

        var request = new UpdateAmmoDetailsRequest(crateId, "RED-TRACE-792", "7.62x39mm", "Tracer");

        await _service.UpdateDetailsAsync(request);

        crate.LotNumber.Should().Be("RED-TRACE-792");

        _ammoRepoMock.Verify(r => r.Update(crate), Times.Once);

        _logRepoMock.Verify(l => l.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Update"
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_Should_DoNothing_When_NoChanges()
    {
        var crate = AmmoCrate.Create("RED-TRACE-762", "7.62x39mm", 800, AmmoType.Tracer);
        var crateId = crate.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);

        var request = new UpdateAmmoDetailsRequest(crateId, "RED-TRACE-762", "7.62x39mm", "Tracer");

        await _service.UpdateDetailsAsync(request);

        _ammoRepoMock.Verify(r => r.Update(It.IsAny<AmmoCrate>()), Times.Never);

        _logRepoMock.Verify(r => r.AddAsync(It.IsAny<OperationLog>()), Times.Never);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Issue_Should_DecreaseQuantity_And_Log_When_StockAvailable()
    {
        var crate = AmmoCrate.Create("M2-AP-BLACK", ".50 BMG", 100, AmmoType.AP);
        var crateId = crate.Id;

        var soldier = Soldier.Create("Frank", "Castle", SoldierRank.Captain);
        var soldierId = soldier.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);
        _soldierRepoMock.Setup(r => r.GetByIdAsync(soldierId)).ReturnsAsync(soldier);

        var request = new IssueAmmoRequest(crateId, 70, soldierId);

        await _service.IssueAmmoAsync(request);

        crate.Quantity.Should().Be(30);

        _ammoRepoMock.Verify(r => r.Update(crate), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Issue" &&
            l.Details.Contains(request.Amount.ToString()) &&
            l.Details.Contains(crate.Type.ToString()) &&
            l.Details.Contains(soldier.LastName) &&
            l.Details.Contains(soldier.FirstName) &&
            l.Details.Contains(crate.LotNumber) &&
            l.Details.Contains(crate.Quantity.ToString())
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Issue_Should_Throw_When_SoldierNotFound()
    {
        var crate = AmmoCrate.Create("SW-357-MAG", ".357 Magnum", 150, AmmoType.HP);
        var crateId = crate.Id;

        var soldierId = Guid.NewGuid();

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);

        var request = new IssueAmmoRequest(crateId, 100, soldierId);

        await _service.Invoking(s => s.IssueAmmoAsync(request))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Soldier with ID {request.SoldierId} not found.");

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Issue_Should_Throw_When_CrateNotFound()
    {
        var soldier = Soldier.Create("Mag", "Sarion", SoldierRank.General);
        var soldierId = soldier.Id;

        var crateId = Guid.NewGuid();

        _soldierRepoMock.Setup(r => r.GetByIdAsync(soldierId)).ReturnsAsync(soldier);

        var request = new IssueAmmoRequest(crateId, 100, soldierId);

        await _service.Invoking(s => s.IssueAmmoAsync(request))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Crate with ID {request.CrateId} not found.");

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Restock_Should_IncreaseQuantity_And_Log()
    {
        var crate = AmmoCrate.Create("NATO-M995", "5.56x45mm", 1000, AmmoType.AP);
        var crateId = crate.Id;

        var soldier = Soldier.Create("Adolf", "Lifter", SoldierRank.Major);
        var soldierId = soldier.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);
        _soldierRepoMock.Setup(r => r.GetByIdAsync(soldierId)).ReturnsAsync(soldier);

        var request = new RestockAmmoRequest(crateId, 400, soldierId);

        await _service.RestockAsync(request);

        crate.Quantity.Should().Be(1400);

        _ammoRepoMock.Verify(r => r.Update(crate), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Restock" &&
            l.Details.Contains(request.Amount.ToString()) &&
            l.Details.Contains(crate.Type.ToString()) &&
            l.Details.Contains(soldier.LastName) &&
            l.Details.Contains(soldier.FirstName) &&
            l.Details.Contains(crate.LotNumber) &&
            l.Details.Contains(crate.Quantity.ToString())
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Audit_Should_AdjustQuantity_And_Log_When_Mismatch()
    {
        var crate = AmmoCrate.Create("COLT-DEF-45", ".45 ACP", 200, AmmoType.HP);
        var crateId = crate.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);

        var request = new AuditAmmoInventoryRequest(crateId, 100);

        await _service.AuditInventoryAsync(request);

        _ammoRepoMock.Verify(r => r.Update(crate), Times.Once);

        _logRepoMock.Verify(r => r.AddAsync(It.Is<OperationLog>(l =>
            l.Action == "Audit" &&
            l.Details.Contains(crate.Quantity.ToString())
        )), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Audit_Should_DoNothing_When_QuantityMatches()
    {
        var crate = AmmoCrate.Create("COLT-DEF-45", ".45 ACP", 200, AmmoType.HP);
        var crateId = crate.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);

        var request = new AuditAmmoInventoryRequest(crateId, 200);

        await _service.AuditInventoryAsync(request);

        _ammoRepoMock.Verify(r => r.Update(It.IsAny<AmmoCrate>()), Times.Never);

        _logRepoMock.Verify(r => r.AddAsync(It.IsAny<OperationLog>()), Times.Never);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task GetCrateById_Should_ReturnDTO_When_Found()
    {
        var crate = AmmoCrate.Create("HK-SubSonic", "9mm", 1000, AmmoType.HP);
        var crateId = crate.Id;

        _ammoRepoMock.Setup(r => r.GetByIdAsync(crateId)).ReturnsAsync(crate);

        var result = await _service.GetCrateByIdAsync(crateId);

        result.Should().NotBeNull();

        result!.Id.Should().Be(crateId);
        result.LotNumber.Should().Be("HK-SubSonic");
        result.Caliber.Should().Be("9mm");
        result.Quantity.Should().Be(1000);
    }

    [Fact]
    public async Task GetCrateById_Should_ReturnNull_When_NotFound()
    {
        var randomId = Guid.NewGuid();

        _ammoRepoMock.Setup(r => r.GetByIdAsync(randomId)).ReturnsAsync((AmmoCrate?)null);

        var result = await _service.GetCrateByIdAsync(randomId);

        result.Should().BeNull();
    }
}
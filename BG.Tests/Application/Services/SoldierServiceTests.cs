using BG.App.DTOs;
using BG.App.Services;
using BG.Domain.Entities;
using BG.Domain.Enums;
using BG.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BG.Tests.Application.Services;

public class SoldierServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ISoldierRepository> _soldierRepoMock;
    private readonly Mock<IWeaponRepository> _weaponRepoMock;
    private readonly Mock<ILogRepository> _logRepoMock;

    private readonly SoldierService _service;

    public SoldierServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _soldierRepoMock = new Mock<ISoldierRepository>();
        _weaponRepoMock = new Mock<IWeaponRepository>();
        _logRepoMock = new Mock<ILogRepository>();

        _uowMock.Setup(u => u.Soldiers).Returns(_soldierRepoMock.Object);
        _uowMock.Setup(u => u.Weapons).Returns(_weaponRepoMock.Object);
        _uowMock.Setup(u => u.Logs).Returns(_logRepoMock.Object);

        _service = new SoldierService(_uowMock.Object);
    }

    [Fact]
    public async Task Create_Should_SaveSoldier_When_DataIsValid()
    {
        var request = new CreateSoldierRequest("John", "Rimbo", "Captain");

        var resultId = await _service.CreateAsync(request);

        _soldierRepoMock.Verify(repo => repo.AddAsync(It.Is<Soldier>(s =>
            s.FirstName == "John" &&
            s.LastName == "Rimbo" &&
            s.Rank == SoldierRank.Captain
        )), Times.Once);

        _uowMock.Verify(uow => uow.CompleteAsync(), Times.Once);

        resultId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_Should_Throw_When_SoldierHasWeapons()
    {
        var soldierId = Guid.NewGuid();
        var soldier = Soldier.Create("John", "Wick", SoldierRank.General);

        _soldierRepoMock.Setup(r => r.GetByIdAsync(soldierId)).ReturnsAsync(soldier);
        _weaponRepoMock.Setup(r => r.HasAnyBySoldierIdAsync(soldierId)).ReturnsAsync(true);

        await _service.Invoking(s => s.DeleteAsync(soldierId)).Should().ThrowAsync<InvalidOperationException>().WithMessage("*weapons*");

        _soldierRepoMock.Verify(r => r.Delete(It.IsAny<Soldier>()), Times.Never);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_Should_UpdateFields_And_Log_When_Changed()
    {
        var soldierId = Guid.NewGuid();
        var soldier = Soldier.Create("James", "Rayan", SoldierRank.Private);

        _soldierRepoMock.Setup(r => r.GetByIdAsync(soldierId)).ReturnsAsync(soldier);

        var request = new UpdateSoldierRequest(soldierId, "James", "Rayan", "Sergeant");

        await _service.UpdateAsync(request);

        soldier.Rank.Should().Be(SoldierRank.Sergeant);

        _soldierRepoMock.Verify(r => r.Update(soldier), Times.Once);

        _logRepoMock.Verify(l => l.AddAsync(It.Is<OperationLog>(log => log.Action == "Update" && log.Details.Contains("Rank"))), Times.Once);

        _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task GetSoldierById_Should_ReturnNull_When_NotFound()
    {
        var randomId = Guid.NewGuid();

        _soldierRepoMock.Setup(r => r.GetByIdAsync(randomId)).ReturnsAsync((Soldier?)null);

        var result = await _service.GetSoldierByIdAsync(randomId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Delete_Should_Throw_KeyNotFound_When_SoldierDoesNotExist()
    {
        var randomId = Guid.NewGuid();

        _soldierRepoMock.Setup(r => r.GetByIdAsync(randomId)).ReturnsAsync((Soldier?)null);

        await _service.Invoking(s => s.DeleteAsync(randomId)).Should().ThrowAsync<KeyNotFoundException>();
    }
}
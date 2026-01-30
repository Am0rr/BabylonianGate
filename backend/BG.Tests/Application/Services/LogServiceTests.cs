using BG.Domain.Interfaces;
using BG.App.Services;
using BG.Domain.Entities;
using BG.Domain.Enums;
using FluentAssertions;
using Moq;
using BG.App.DTOs;
using System.Linq.Expressions;

namespace BG.Tests.Application.Services;

public class LogServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ILogRepository> _logRepoMock;

    private readonly LogService _service;

    public LogServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _logRepoMock = new Mock<ILogRepository>();

        _uowMock.Setup(u => u.Logs).Returns(_logRepoMock.Object);

        _service = new LogService(_uowMock.Object);
    }

    [Fact]
    public async Task GetLogById_Should_ReturnMappedDTO_When_Found()
    {
        var log = OperationLog.Create("Issue", "fefefe");
        var logId = log.Id;

        _logRepoMock.Setup(r => r.GetByIdAsync(logId)).ReturnsAsync(log);

        var result = await _service.GetLogByIdAsync(logId);

        result.Should().NotBeNull();

        result!.Id.Should().Be(logId);
        result.Action.Should().Be("Issue");
        result.Details.Should().Be("fefefe");
    }

    [Fact]
    public async Task GetLogById_Should_ReturnNull_When_NotFound()
    {
        var randomId = Guid.NewGuid();

        _logRepoMock.Setup(r => r.GetByIdAsync(randomId)).ReturnsAsync((OperationLog?)null);

        var result = await _service.GetLogByIdAsync(randomId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetHistory_Should_ReturnSortedDTOs_When_LogsExist()
    {
        var randomId = Guid.NewGuid();

        var logOld = OperationLog.Create("Return", "Oldest Log", randomId);
        await Task.Delay(20);

        var logMid = OperationLog.Create("Return", "Middle Log", randomId);
        await Task.Delay(20);

        var logNew = OperationLog.Create("Return", "Newest Log", randomId);

        var logsFromDb = new List<OperationLog> { logOld, logMid, logNew };

        _logRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OperationLog, bool>>>()))
            .ReturnsAsync(logsFromDb);

        var result = await _service.GetHistoryByEntityIdAsync(randomId);

        result.Should().HaveCount(3);

        result.First().Details.Should().Be("Newest Log");
        result.Last().Details.Should().Be("Oldest Log");

        result.All(x => x.RelatedEntityId == randomId).Should().BeTrue();
    }

    [Fact]
    public async Task GetHistory_Should_ReturnEmpty_When_NoLogs()
    {
        var randomId = Guid.NewGuid();

        var logsFromDb = new List<OperationLog>();

        _logRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OperationLog, bool>>>()))
            .ReturnsAsync(logsFromDb);

        var result = await _service.GetHistoryByEntityIdAsync(randomId);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRecent_Should_UseDefaultCount_When_NoArgument()
    {
        _logRepoMock.Setup(r => r.GetRecentAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<OperationLog>());

        await _service.GetRecentLogsAsync();

        _logRepoMock.Verify(r => r.GetRecentAsync(100), Times.Once);
    }

    [Fact]
    public async Task GetRecent_Should_PassCustomCount_And_MapResults()
    {
        int count = 50;

        var logs = new List<OperationLog>
        {
            OperationLog.Create("Audit", "Check 1"),
            OperationLog.Create("Audit", "Check 2")
        };

        _logRepoMock.Setup(r => r.GetRecentAsync(count)).ReturnsAsync(logs);

        var result = await _service.GetRecentLogsAsync(count);

        _logRepoMock.Verify(r => r.GetRecentAsync(count), Times.Once);

        result.Should().HaveCount(2);
        result.First().Action.Should().Be("Audit");
    }
}
using System.Collections.Specialized;
using BG.Domain.Entities;
using BG.Domain.Enums;
using FluentAssertions;

namespace BG.Tests.Domain.Entities;

public class OperationLogTests
{
    [Fact]
    public void Create_Should_ReturnOperationLog_When_DataIsValid()
    {
        var action = "Create";
        var details = "Claimed weapon ...";

        var log = OperationLog.Create(action, details);

        log.Action.Should().Be(action);
        log.Details.Should().Be(details);
    }

    [Theory]
    [InlineData("", "Claimed weapon ...")]
    [InlineData("Create", "")]
    [InlineData(null, "Claimed weapon ...")]
    [InlineData("Create", null)]
    public void Create_Should_Throw_When_DataIsInvalid(string? action, string? details)
    {
        Action act = () => OperationLog.Create(action!, details!);

        act.Should().Throw<ArgumentException>();
    }
}
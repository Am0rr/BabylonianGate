using BG.Domain.Entities;
using BG.Domain.Enums;
using FluentAssertions;

namespace BG.Tests.Domain.Entities;

public class SoldierTests
{
    [Fact]
    public void Create_Should_ReturnSoldier_When_DataIsValid()
    {
        var firstName = "Benjamin";
        var lastName = "Poindexter";
        var rank = SoldierRank.Captain;

        var soldier = Soldier.Create(firstName, lastName, rank);

        soldier.FirstName.Should().Be(firstName);
        soldier.LastName.Should().Be(lastName);
        soldier.Rank.Should().Be(rank);
    }

    [Theory]
    [InlineData("", "Poindexter")]
    [InlineData("Benjamin", "")]
    public void Create_Should_Throw_When_DataIsInvalid(string? firstName, string? lastName)
    {
        Action act = () => Soldier.Create(firstName!, lastName!, SoldierRank.Captain);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateName_Should_UpdateFields_When_DataIsValid()
    {
        var soldier = Soldier.Create("Steve", "Rogers", SoldierRank.Private);

        soldier.UpdateName("Captain", "America");

        soldier.FirstName.Should().Be("Captain");
        soldier.LastName.Should().Be("America");
    }

    [Theory]
    [InlineData("", "Rogers")]
    [InlineData("Steve", "")]
    [InlineData(null, "Rogers")]
    [InlineData("Steve", null)]
    public void UpdateName_Should_Throw_When_DataIsInvalid(string? firstName, string? lastName)
    {
        var soldier = Soldier.Create("Steve", "Rogers", SoldierRank.Private);

        Action act = () => soldier.UpdateName(firstName!, lastName!);

        act.Should().Throw<ArgumentException>().WithMessage("Fist/Last Name cannot be empty.");
    }

    [Fact]
    public void UpdateRank_Should_ChangeRank()
    {
        var soldier = Soldier.Create("Bucky", "Barnes", SoldierRank.Sergeant);

        soldier.UpdateRank(SoldierRank.Captain);

        soldier.Rank.Should().Be(SoldierRank.Captain);
    }
}
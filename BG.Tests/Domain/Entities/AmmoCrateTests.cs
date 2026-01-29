using BG.Domain.Entities;
using BG.Domain.Enums;
using FluentAssertions;

namespace BG.Tests.Domain.Entities;

public class AmmoTests
{
    [Fact]
    public void Create_Should_ReturnAmmo_When_DataIsValid()
    {
        var lotNumber = "BLACK-TIP-308";
        var caliber = "7.62x51mm";
        var quantity = 1000;
        var type = AmmoType.AP;

        var crate = AmmoCrate.Create(lotNumber, caliber, quantity, type);

        crate.LotNumber.Should().Be(lotNumber);
        crate.Caliber.Should().Be(caliber);
        crate.Quantity.Should().Be(quantity);
        crate.Type.Should().Be(type);
    }

    [Theory]
    [InlineData("", "7.62x51mm", 1000)]
    [InlineData("BLACK-TIP-308", "", 1000)]
    [InlineData("BLACK-TIP-308", "7.62x51mm", -1)]
    [InlineData(null, "7.62x51mm", 1000)]
    public void Create_Should_Throw_When_DataIsInvalid(string? lotNumber, string caliber, int quantity)
    {
        Action act = () => AmmoCrate.Create(lotNumber!, caliber, quantity, AmmoType.AP);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Issue_Should_Update_When_Valid()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        crate.Issue(250);

        crate.Quantity.Should().Be(750);
    }

    [Fact]
    public void Issue_Should_Throw_When_AmountSmallerThanZero()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        Action act = () => crate.Issue(-500);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Issue amount must be greater than zero.");
    }

    [Fact]
    public void Issue_Should_Throw_When_QuantityIsNotEnough()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        Action act = () => crate.Issue(1800);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Restock_Should_Update_When_Valid()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);
        crate.Issue(900);

        crate.Restock(1500);

        crate.Quantity.Should().Be(1600);
    }

    [Fact]
    public void Restock_Should_Throw_When_AmountIsSmallerThanZero()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        Action act = () => crate.Restock(-5);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Amount must be positive");
    }

    [Fact]
    public void AdjustQuantity_Should_Update_When_Valid()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        crate.AdjustQuantity(500);

        crate.Quantity.Should().Be(500);
    }

    [Fact]
    public void AdjustQuantity_Should_Throw_When_AmountIsSmallerThanZero()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        Action act = () => crate.AdjustQuantity(-5);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Quanity must be positive.");
    }

    [Fact]
    public void CorrectCaliber_Should_Update_When_Valid()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        crate.CorrectCaliber("6mm");

        crate.Caliber.Should().Be("6mm");
    }

    [Fact]
    public void CorrectCaliber_Should_Throw_When_Empty()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        Action act = () => crate.CorrectCaliber(" ");

        act.Should().Throw<ArgumentException>()
            .WithMessage("Caliber cannot be empty.");
    }

    [Fact]
    public void CorrectLotNumber_Should_Update_When_Valid()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        crate.CorrectLotNumber("GLOCK-DEFF-9");

        crate.LotNumber.Should().Be("GLOCK-DEFF-9");
    }

    [Fact]
    public void CorrectLotNumber_Should_Throw_When_Empty()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        Action act = () => crate.CorrectLotNumber(" ");

        act.Should().Throw<ArgumentException>()
            .WithMessage("Lot Number cannot be empty.");
    }

    [Fact]
    public void CorrectType_Should_Update_When_Valid()
    {
        var crate = AmmoCrate.Create("GLOCK-DEF-9", "9mm", 1000, AmmoType.HP);

        crate.CorrectType(AmmoType.FMJ);

        crate.Type.Should().Be(AmmoType.FMJ);
    }
}
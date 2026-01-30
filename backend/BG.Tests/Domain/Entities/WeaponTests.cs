using BG.Domain.Entities;
using BG.Domain.Enums;
using FluentAssertions;

namespace BG.Tests.Domain.Entities;

public class WeaponTests
{
    [Fact]
    public void Create_Should_ReturnWeapon_When_DataIsValid()
    {
        var codename = "AK-47";
        var serial = "SN-12345";
        var caliber = "7.62x39mm";
        var type = WeaponType.Rifle;

        var weapon = Weapon.Create(codename, serial, caliber, type);

        weapon.Codename.Should().Be(codename);
        weapon.SerialNumber.Should().Be(serial);
        weapon.Caliber.Should().Be(caliber);
        weapon.Type.Should().Be(type);
        weapon.Condition.Should().Be(100.0);
        weapon.Status.Should().Be(WeaponStatus.InStorage);
        weapon.IssuedToSoldierId.Should().BeNull();
    }

    [Theory]
    [InlineData("", "SN-1", "5.56")]
    [InlineData("M4", "", "5.56")]
    [InlineData("M4", "SN-1", "")]
    [InlineData(null, "SN-1", "5.56")]
    public void Create_Should_Throw_When_DataIsInvalid(string? codename, string? serial, string? caliber)
    {
        Action act = () => Weapon.Create(codename!, serial!, caliber!, WeaponType.Rifle);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void IssueTo_Should_SetStatusDeployed_And_AssignSoldier()
    {
        var weapon = Weapon.Create("Glock", "G-001", "9mm", WeaponType.Pistol);
        var soldierId = Guid.NewGuid();

        weapon.IssueTo(soldierId);

        weapon.Status.Should().Be(WeaponStatus.Deployed);
        weapon.IssuedToSoldierId.Should().Be(soldierId);
    }

    [Fact]
    public void IssueTo_Should_Throw_When_Not_InStorage()
    {
        var weapon = Weapon.Create("Glock", "G-001", "9mm", WeaponType.Pistol);
        weapon.MarkAsMissing();

        Action act = () => weapon.IssueTo(Guid.NewGuid());

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Weapon is not in storage.");
    }

    [Fact]
    public void IssueTo_Should_Throw_When_WeaponIsBroken()
    {
        var weapon = Weapon.Create("M16", "VN-001", "5.56", WeaponType.Rifle);
        var soldierId = Guid.NewGuid();

        weapon.IssueTo(soldierId);
        weapon.ApplyWear(1000);
        weapon.ReturnToStorage();

        Action act = () => weapon.IssueTo(soldierId);

        weapon.Condition.Should().Be(0);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot issue broken weapon.");
    }

    [Fact]
    public void ApplyWear_Should_ReduceCondition_Correctly()
    {
        var weapon = Weapon.Create("M249", "SAW-01", "5.56", WeaponType.Rifle);
        weapon.IssueTo(Guid.NewGuid());

        weapon.ApplyWear(100);

        weapon.Condition.Should().Be(90.0);
    }

    [Fact]
    public void ApplyWear_Should_NotGoBelowZero()
    {
        var weapon = Weapon.Create("M249", "SAW-01", "5.56", WeaponType.Rifle);
        weapon.IssueTo(Guid.NewGuid());

        weapon.ApplyWear(2000);

        weapon.Condition.Should().Be(0.0);
    }

    [Fact]
    public void ApplyWear_Should_Throw_When_NotDeployed()
    {
        var weapon = Weapon.Create("RPG", "R-1", "Rocket", WeaponType.Launcher);

        Action act = () => weapon.ApplyWear(100);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Cannot apply wear. Weapon status is '{weapon.Status}', but must be 'Deployed'.");
    }

    [Fact]
    public void ReturnToStorage_Should_ClearSoldier_And_SetStatus()
    {
        var weapon = Weapon.Create("AWP", "UK-1", ".338", WeaponType.Sniper);
        weapon.IssueTo(Guid.NewGuid());

        weapon.ReturnToStorage();

        weapon.Status.Should().Be(WeaponStatus.InStorage);
        weapon.IssuedToSoldierId.Should().BeNull();
    }

    [Fact]
    public void ReturnToStorage_Should_Repair_When_ReturningFromMaintenance()
    {
        var weapon = Weapon.Create("Rusty", "OLD-1", "9mm", WeaponType.Pistol);
        weapon.SendToMaintenance();

        weapon.ReturnToStorage();

        weapon.Status.Should().Be(WeaponStatus.InStorage);
        weapon.Condition.Should().Be(100);
    }

    [Fact]
    public void SendToMaintenance_Should_Throw_When_Deployed()
    {
        var weapon = Weapon.Create("Scar-L", "FN-01", "5.56", WeaponType.Rifle);
        weapon.IssueTo(Guid.NewGuid());

        Action act = () => weapon.SendToMaintenance();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot send active weapon to maintenance. Return from soldier first.");
    }

    [Fact]
    public void ChangeCodeName_Should_Update_When_InStorage()
    {
        var weapon = Weapon.Create("Typo", "SN-1", "5.56", WeaponType.Rifle);

        weapon.ChangeCodeName("CorrectName");
        weapon.Codename.Should().Be("CorrectName");
    }

    [Fact]
    public void ChangeCodeName_Should_Throw_When_Deployed()
    {
        var weapon = Weapon.Create("M4", "SN-1", "5.56", WeaponType.Rifle);
        weapon.IssueTo(Guid.NewGuid());

        Action act = () => weapon.ChangeCodeName("CorrectName");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Cannot edit weapon details while it is in status: {weapon.Status}. Return to storage first.");
    }

    [Fact]
    public void CorrectSerialNumber_Should_Update_When_InStorage()
    {
        var weapon = Weapon.Create("M4", "Wrong-SN", "5.56", WeaponType.Rifle);
        weapon.CorrectSerialNumber("FixedSerialNumber");
        weapon.SerialNumber.Should().Be("FixedSerialNumber");
    }

    [Fact]
    public void CorrectCaliber_Should_Update_When_InStorage()
    {
        var weapon = Weapon.Create("M4", "Wrong-SN", "5.56", WeaponType.Rifle);
        weapon.CorrectCaliber("FisedCaliber");
        weapon.Caliber.Should().Be("FisedCaliber");
    }
}
using BG.Domain.Enums;
using BG.Domain.Common;

namespace BG.Domain.Entities;

public class Weapon : Entity
{
    public string Codename { get; private set; } = string.Empty;
    public string SerialNumber { get; private set; } = string.Empty;
    public string Caliber { get; private set; } = string.Empty;
    public WeaponType Type { get; private set; }

    public double Condition { get; private set; }
    public WeaponStatus Status { get; private set; }

    public Guid? IssuedToSoldierId { get; private set; }
    private Weapon()
    {
    }
    private Weapon(string codeName, string serialNumber, string caliber, WeaponType type)
    {
        Codename = codeName;
        SerialNumber = serialNumber;
        Type = type;
        Caliber = caliber;
        Condition = 100.0;
        Status = WeaponStatus.InStorage;
    }

    public static Weapon Create(string codeName, string serialNumber, string caliber, WeaponType type)
    {
        if (string.IsNullOrWhiteSpace(serialNumber))
            throw new ArgumentException("Serial Number is required", nameof(serialNumber));
        if (string.IsNullOrWhiteSpace(codeName))
            throw new ArgumentException("Code Name is required", nameof(codeName));
        if (string.IsNullOrWhiteSpace(caliber))
            throw new ArgumentException("Caliber is required", nameof(caliber));

        return new Weapon(codeName, serialNumber, caliber, type);
    }

    public void ApplyWear(int roundsFired)
    {
        if (roundsFired < 0) throw new ArgumentException("Rounds cannot be negative.");
        if (roundsFired == 0) return;
        if (Status != WeaponStatus.Deployed)
        {
            throw new InvalidOperationException($"Cannot apply wear. Weapon status is '{Status}', but must be 'Deployed'.");
        }

        var damage = roundsFired * 0.1;
        Condition = Math.Max(0, Condition - damage);
    }

    public void ChangeCodeName(string newCodeName)
    {
        if (string.IsNullOrWhiteSpace(newCodeName))
        {
            throw new ArgumentException("Code Name is invalid.");
        }

        EnsureEditable();

        Codename = newCodeName;
    }

    public void CorrectSerialNumber(string fixedSerialNumber)
    {
        if (string.IsNullOrWhiteSpace(fixedSerialNumber))
            throw new ArgumentException("Serial Number cannot be empty.");


        EnsureEditable();

        SerialNumber = fixedSerialNumber;
    }

    public void CorrectCaliber(string correctedCaliber)
    {
        if (string.IsNullOrWhiteSpace(correctedCaliber))
        {
            throw new ArgumentException("Caliber cannot be empty.");
        }

        EnsureEditable();

        Caliber = correctedCaliber;
    }

    private void EnsureEditable()
    {
        if (Status != WeaponStatus.InStorage)
        {
            throw new InvalidOperationException($"Cannot edit weapon details while it is in status: {Status}. Return to storage first.");
        }
    }

    public void IssueTo(Guid soldierId)
    {
        if (Status != WeaponStatus.InStorage)
            throw new InvalidOperationException("Weapon is not in storage.");

        if (Condition <= 0)
            throw new InvalidOperationException("Cannot issue broken weapon.");

        Status = WeaponStatus.Deployed;
        IssuedToSoldierId = soldierId;
    }

    public void ReturnToStorage()
    {
        if (Status == WeaponStatus.InStorage) return;

        if (Status == WeaponStatus.Maintenance)
        {
            Condition = 100.0;
        }

        Status = WeaponStatus.InStorage;
        IssuedToSoldierId = null;
    }

    public void MarkAsMissing()
    {
        Status = WeaponStatus.Missing;
    }

    public void SendToMaintenance()
    {
        if (Status == WeaponStatus.Deployed)
            throw new InvalidOperationException("Cannot send active weapon to maintenance. Return from soldier first.");

        if (Status == WeaponStatus.Missing)
            throw new InvalidOperationException("Cannot repair missing weapon.");

        Status = WeaponStatus.Maintenance;
        IssuedToSoldierId = null;
    }
}
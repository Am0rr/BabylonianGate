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

    public static (Weapon? Weapon, string Error) Create(string codeName, string serialNumber, string caliber, WeaponType type)
    {
        if (string.IsNullOrWhiteSpace(serialNumber))
            return (null, "Serial Number is required");
        if (string.IsNullOrWhiteSpace(codeName))
            return (null, "Code Number is required");
        if (string.IsNullOrWhiteSpace(caliber))
            return (null, "Caliber is required");

        return (new Weapon(codeName, serialNumber, caliber, type), string.Empty);
    }

    public void Fire(int rounds)
    {
        if (rounds <= 0) throw new ArgumentException("Rounds must be positive.");
        if (Status == WeaponStatus.Maintenance) throw new InvalidOperationException("Cannot fire weapon in maintenance.");
        if (Status == WeaponStatus.InStorage) throw new InvalidOperationException("Cannot fire weapon that is in storage.");

        var damage = rounds * 0.1;
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

    public void ChangeStatus(WeaponStatus newStatus)
    {
        if (newStatus == WeaponStatus.Deployed && Condition <= 0)
        {
            throw new InvalidOperationException("Cannot activate a broken weapon.");
        }

        if (Status == WeaponStatus.Maintenance && newStatus == WeaponStatus.InStorage)
        {
            Condition = 100.0;
        }

        if (Status == WeaponStatus.Missing && newStatus == WeaponStatus.Deployed)
        {
            throw new InvalidOperationException("Cannot deploy missing weapon. Return it to storage first.");
        }

        Status = newStatus;
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

        Status = WeaponStatus.Active;
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
        if (Status == WeaponStatus.Active)
            throw new InvalidOperationException("Cannot send active weapon to maintenance. Return from soldier first.");

        if (Status == WeaponStatus.Missing)
            throw new InvalidOperationException("Cannot repair missing weapon.");

        Status = WeaponStatus.Maintenance;
        IssuedToSoldierId = null;
    }
}
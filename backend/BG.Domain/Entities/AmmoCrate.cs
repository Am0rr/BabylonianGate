using BG.Domain.Common;
using BG.Domain.Enums;

namespace BG.Domain.Entities;

public class AmmoCrate : Entity
{
    public string LotNumber { get; private set; } = string.Empty;
    public string Caliber { get; private set; } = string.Empty;
    public AmmoType Type { get; private set; }
    public int Quantity { get; private set; }

    private AmmoCrate()
    {

    }
    private AmmoCrate(string lotNumber, string caliber, int quantity, AmmoType type)
    {
        LotNumber = lotNumber;
        Caliber = caliber;
        Type = type;
        Quantity = quantity;
    }

    public static AmmoCrate Create(string lotNumber, string caliber, int quantity, AmmoType type)
    {
        if (string.IsNullOrWhiteSpace(lotNumber))
            throw new ArgumentException("Lot Number is required.", nameof(lotNumber));

        if (string.IsNullOrWhiteSpace(caliber))
            throw new ArgumentException("Caliber is required.", nameof(caliber));

        if (quantity < 0)
            throw new ArgumentException("Ammo quantity cannot be negative.", nameof(quantity));

        return new AmmoCrate(lotNumber, caliber, quantity, type);
    }

    public void Issue(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Issue amount must be greater than zero.");

        if (amount > Quantity)
            throw new InvalidOperationException($"Not enough ammo. Requested: {amount}, Available: {Quantity}");

        Quantity -= amount;
    }

    public void Restock(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive");

        Quantity += amount;
    }

    public void AdjustQuantity(int actualQuantity)
    {
        if (actualQuantity < 0) throw new ArgumentException("Quanity must be positive.");

        Quantity = actualQuantity;
    }

    public void CorrectCaliber(string newCaliber)
    {
        if (string.IsNullOrWhiteSpace(newCaliber))
            throw new ArgumentException("Caliber cannot be empty.");

        if (Caliber == newCaliber) return;

        Caliber = newCaliber;
    }

    public void CorrectType(AmmoType newType)
    {
        if (Type == newType) return;

        Type = newType;
    }

    public void CorrectLotNumber(string newLotNumber)
    {
        if (string.IsNullOrWhiteSpace(newLotNumber))
            throw new ArgumentException("Lot Number cannot be empty.");

        if (LotNumber == newLotNumber) return;

        LotNumber = newLotNumber;
    }
}
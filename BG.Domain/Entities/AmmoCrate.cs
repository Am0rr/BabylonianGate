using BG.Domain.Common;
using BG.Domain.Enums;

namespace BG.Domain.Entities;

public class AmmoCrate : Entity
{
    public string Caliber { get; private set; } = string.Empty;
    public AmmoType Type { get; private set; }
    public int Quantity { get; private set; }

    private AmmoCrate()
    {

    }
    private AmmoCrate(string caliber, AmmoType type, int quantity)
    {
        Caliber = caliber;
        Type = type;
        Quantity = quantity;
    }

    public static (AmmoCrate? Crate, string Error) Create(string caliber, AmmoType type, int quantity)
    {
        if (string.IsNullOrWhiteSpace(caliber))
            return (null, "Caliber is required.");

        if (quantity < 0)
            return (null, "Ammo quantity cannot be negative.");


        return (new AmmoCrate(caliber, type, quantity), string.Empty);
    }

    public void TakeAmmo(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        if (amount > Quantity)
            throw new InvalidOperationException($"Not enough ammo. Requested: {amount}, Available: {Quantity}");

        Quantity -= amount;
    }

    public void RestockAmmo(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive");

        Quantity += amount;
    }

    public void CorrectQuanity(int actualQuanity)
    {
        if (actualQuanity < 0) throw new ArgumentException("Quanity must be positive.");

        Quantity = actualQuanity;
    }
}
using BG.Domain.Enums;

namespace BG.App.DTOs;

public record SoldierResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Rank
);
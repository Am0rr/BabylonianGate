

using BG.Domain.Enums;

namespace BG.App.DTOs;

public record UpdateSoldierRequest(
    Guid Id,
    string FirstName,
    string LastName,
    Rank Rank
);
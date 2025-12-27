using BG.Domain.Enums;

namespace BG.App.DTOs;

public record CreateSoldierRequest(
    string FirstName,
    string LastName,
    Rank Rank
);


namespace BG.App.DTOs;

public record LogResponse(
    Guid Id,
    string Action,
    string Details,
    Guid? RelatedEntityId,
    DateTime CreatedAt
);


namespace BG.App.DTOs;

public record LogResponse(
    Guid Id,
    string Action,
    string Details,
    DateTime CreatedAt,
    Guid? RelatedEntityId,
    Guid? OperatorId
);
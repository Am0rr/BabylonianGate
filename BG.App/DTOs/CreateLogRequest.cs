

namespace BG.App.DTOs;

public record CreateLogRequest(
    string Action,
    string Details,
    Guid? RelatedEntityId
);
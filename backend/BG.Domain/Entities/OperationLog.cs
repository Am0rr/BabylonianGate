

using BG.Domain.Common;

namespace BG.Domain.Entities;

public class OperationLog : Entity
{
    public string Action { get; private set; } = string.Empty;
    public string Details { get; private set; } = string.Empty;
    public Guid? RelatedEntityId { get; private set; }
    public Guid? OperatorId { get; private set; }

    private OperationLog()
    {

    }
    private OperationLog(string action, string details, Guid? relatedEntityId, Guid? operatorId)
    {
        Action = action;
        Details = details;
        RelatedEntityId = relatedEntityId;
        OperatorId = operatorId;
    }

    public static OperationLog Create(string action, string details, Guid? relatedEntityId = null, Guid? operatorId = null)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Log action cannot be empty.", nameof(action));
        if (string.IsNullOrWhiteSpace(details))
            throw new ArgumentException("Log details cannot be empty", nameof(details));

        return new OperationLog(action, details, relatedEntityId, operatorId);
    }
}
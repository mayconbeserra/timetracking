using System;

namespace Visma.TimeTracking.EventSourcing.Events
{
    public interface IDomainEvent
    {
        string AggregateId { get; set; }
        string Id { get; }
        string Type { get; set; }
        string CorrelationId { get; set; }
        DateTime CreatedAt { get; set; }
        string CreatorId { get; set; }
        long Version { get; set; }
    }
}
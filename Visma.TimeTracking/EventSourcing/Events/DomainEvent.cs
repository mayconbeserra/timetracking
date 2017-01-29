using System;

namespace Visma.TimeTracking.EventSourcing.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public string AggregateId { get; set; }
        public string Id { get; }
        public string Type { get; set; }
        public string CorrelationId { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long Version { get; set; }

        protected DomainEvent(string creatorId, string correlationId)
        {
            Id = Guid.NewGuid().ToString();
            Type = GetType().Name;
            CreatedAt = DateTime.UtcNow;
            CreatorId = creatorId;
            CorrelationId = correlationId;
        }
    }
}
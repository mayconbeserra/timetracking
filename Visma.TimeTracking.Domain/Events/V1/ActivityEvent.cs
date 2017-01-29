using Visma.TimeTracking.EventSourcing.Events;

namespace Visma.TimeTracking.Domain.Events.V1
{
    public abstract class ActivityEvent : DomainEvent
    {
        public string ProjectId { get; protected set; }
        public string Description { get; protected set; }

        protected ActivityEvent(string creatorId = null, string correlationId = null) : base(creatorId, correlationId)
        {
        }
    }
}
using System;

namespace Visma.TimeTracking.Domain.Events.V1
{
    public class ActivityStarted : ActivityEvent
    {
        public DateTime StartDate { get; }

        public ActivityStarted(string aggregateId,
            string projectId,
            DateTime startDate,
            string description,
            string creatorId,
            string correlationId) : base(creatorId, correlationId)
        {
            AggregateId = aggregateId;
            ProjectId = projectId;
            StartDate = startDate;
            Description = description;
        }
    }
}
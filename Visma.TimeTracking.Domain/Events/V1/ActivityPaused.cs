using System;

namespace Visma.TimeTracking.Domain.Events.V1
{
    public class ActivityPaused : ActivityEvent
    {
        public ActivityPaused(
            string projectId,
            DateTime endDate,
            double totalInMinutes,
            string description,
            string creator,
            string correlationId) : base(creator, correlationId)
        {
            ProjectId = projectId;
            EndDate = endDate;
            TotalInMinutes = totalInMinutes;
            Description = description;
        }

        public DateTime EndDate { get; }
        public double TotalInMinutes { get; }
    }
}
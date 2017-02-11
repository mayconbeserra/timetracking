using System;

namespace Visma.TimeTracking.Domain.Events.V1
{
    public class ActivityAdjusted : ActivityEvent
    {
        public ActivityAdjusted(
            string projectId,
            DateTime startDate,
            DateTime endDate,
            double totalInMinutes,
            string description,
            string creator,
            string correlationId) : base(creator, correlationId)
        {
            ProjectId = projectId;
            StartDate = startDate;
            EndDate = endDate;
            TotalInMinutes = totalInMinutes;
            Description = description;
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public double TotalInMinutes { get; set; }
    }
}
using System;

namespace Visma.TimeTracking.Domain.Events.V1
{
    public class ActivityAdjusted : ActivityEvent
    {
        public ActivityAdjusted(DateTime startDate,
            DateTime endDate,
            double totalInMinutes,
            string description,
            string creator,
            string correlationId) : base(creator, correlationId)
        {
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
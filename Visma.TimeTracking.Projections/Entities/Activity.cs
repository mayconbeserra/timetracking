using System;

namespace Visma.TimeTracking.Projections.Entities
{
    public class Activity : Entity
    {
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
using System;

namespace Visma.TimeTracking.API.Models
{
    public class ActivityInput
    {
        public string ProjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
    }
}
using System.Collections.Generic;

namespace Visma.TimeTracking.Projections.Entities
{
    public class Project : Entity
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
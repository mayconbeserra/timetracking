using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.Projections.Entities;

namespace Visma.TimeTracking.Projections.Contexts
{
    public class TimerTrackingDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }

        public TimerTrackingDbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public TimerTrackingDbContext()
        {

        }
    }
}
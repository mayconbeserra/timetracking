using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.EventSourcing.Models;

namespace Visma.TimeTracking.EventSourcing.Db
{
    public class EventStoreDbContext : DbContext
    {
        public DbSet<Stream> Streams { get; set; }
        public DbSet<Event> Events { get; set; }

        public EventStoreDbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected EventStoreDbContext()
        {
        }
    }
}
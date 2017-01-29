using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Visma.TimeTracking.EventSourcing.Events;
using Visma.TimeTracking.Projections.Contexts;

namespace Visma.TimeTracking.Projections.Handlers
{
    public abstract class EventHandler
    {
        protected TimerTrackingDbContext Db { get; }

        protected EventHandler(TimerTrackingDbContext db)
        {
            Db = db;
        }

        protected async Task SaveChangesAsync(IDomainEvent @event)
        {
            try
            {
                await Db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException == null) throw;

                var inner = e.InnerException as PostgresException;
                if (inner == null) throw;
                if (inner.SqlState != "23505") throw;
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}
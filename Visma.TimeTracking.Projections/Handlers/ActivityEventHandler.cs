using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.Domain.Events.V1;
using Visma.TimeTracking.EventSourcing.Handlers;
using Visma.TimeTracking.Projections.Contexts;
using Visma.TimeTracking.Projections.Extensions;

namespace Visma.TimeTracking.Projections.Handlers
{
    public sealed class ActivityEventHandler : EventHandler, IEventHandler<ActivityEvent>
    {
        public ActivityEventHandler(TimerTrackingDbContext db) : base(db)
        {
        }

        public async Task Handle(ActivityEvent @event)
        {
            await Handle((dynamic) @event);
        }

        private async Task Handle(ActivityStarted @event)
        {
            var activity = @event.ToActivity();
            if (!Db.Activities.Any(c => c.Id == activity.Id))
                Db.Activities.Add(activity);

            await SaveChangesAsync(@event);
        }

        private async Task Handle(ActivityPaused @event)
        {
            var activity = await Db.Activities.FirstOrDefaultAsync(x => x.Id == @event.AggregateId);

            activity.EndDate = @event.EndDate;
            activity.Description = @event.Description;
            activity.ModifiedAt = @event.CreatedAt;
            activity.Version = @event.Version;

            await SaveChangesAsync(@event);
        }

        private async Task Handle(ActivityAdjusted @event)
        {
            var activity = await Db.Activities.FirstOrDefaultAsync(x => x.Id == @event.AggregateId);

            activity.StartDate = @event.StartDate;
            activity.EndDate = @event.EndDate;
            activity.Description = @event.Description;
            activity.ModifiedAt = @event.CreatedAt;
            activity.Version = @event.Version;

            await SaveChangesAsync(@event);
        }
    }
}
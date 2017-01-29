using System;
using Visma.TimeTracking.Domain.Events.V1;
using Visma.TimeTracking.Projections.Entities;

namespace Visma.TimeTracking.Projections.Extensions
{
    public static class ActivityEventExtensions
    {
        public static Activity ToActivity(this ActivityStarted @event)
        {
            return new Activity
            {
                Id = @event.AggregateId,
                StartDate = @event.StartDate,
                Version = @event.Version,
                ProjectId = @event.ProjectId,
                CreatedAt = @event.CreatedAt,
                ModifiedAt = DateTime.UtcNow,
                CreatorId = @event.CreatorId,
                Description = @event.Description,
            };
        }

        public static Activity ToActivity(this ActivityPaused @event)
        {
            return new Activity
            {
                Id = @event.AggregateId,
                EndDate = @event.EndDate,
                Description = @event.Description,
                ModifiedAt = @event.CreatedAt
            };
        }
    }
}
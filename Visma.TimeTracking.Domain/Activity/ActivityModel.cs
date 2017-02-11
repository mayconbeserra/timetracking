using System;
using Visma.TimeTracking.Domain.Events.V1;
using Visma.TimeTracking.EventSourcing;

namespace Visma.TimeTracking.Domain.Activity
{
    public sealed class ActivityModel : AggregateRoot<ActivityState>
    {
        private bool Exists => !string.IsNullOrEmpty(Id);

        public static string GenerateId(string project)
        {
            return $"vis.proj-{project}.activity";
        }

        public ActivityModel Start(string projectId,
            DateTime startDate,
            string description,
            string creatorId,
            string correlationId)
        {
            if (Exists) throw new InvalidOperationException();
            if (startDate == DateTime.MaxValue) throw new ArgumentNullException(nameof(startDate));
            if (startDate == DateTime.MinValue) throw new ArgumentNullException(nameof(startDate));
            if (string.IsNullOrEmpty(projectId)) throw new ArgumentNullException(nameof(projectId));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            var aggregateId = Guid.NewGuid().ToString();

            Apply(new ActivityStarted(aggregateId, projectId, startDate, description, creatorId, correlationId));

            return this;
        }

        public ActivityModel Pause(DateTime endDate,
            string description,
            string creator,
            string correlationId)
        {
            if (!Exists) throw new InvalidOperationException("Activity is not created");
            if (State.EndDate.HasValue) throw new InvalidOperationException("This activity was already paused");
            if (endDate == DateTime.MaxValue) throw new ArgumentNullException(nameof(endDate));
            if (endDate == DateTime.MinValue) throw new ArgumentNullException(nameof(endDate));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            Apply(new ActivityPaused(
                State.ProjectId,
                endDate,
                CalculateMinutes(State.StartDate, endDate),
                description,
                creator,
                correlationId
            ));

            return this;
        }

        public ActivityModel Adjust(DateTime startDate,
            DateTime endDate,
            string description,
            string creator,
            string correlationId)
        {
            if (!Exists) throw new InvalidOperationException();
            if (startDate == DateTime.MaxValue) throw new ArgumentNullException(nameof(startDate));
            if (startDate == DateTime.MinValue) throw new ArgumentNullException(nameof(startDate));
            if (endDate == DateTime.MaxValue) throw new ArgumentNullException(nameof(endDate));
            if (endDate == DateTime.MinValue) throw new ArgumentNullException(nameof(endDate));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            Apply(
                new ActivityAdjusted(
                    State.ProjectId,
                    startDate,
                    endDate,
                    CalculateMinutes(startDate, endDate),
                    description,
                    creator,
                    correlationId)
            );

            return this;
        }

        private static double CalculateMinutes(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).TotalMinutes;
        }
    }
}
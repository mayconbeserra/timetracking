using System;
using Visma.TimeTracking.Domain.Events.V1;
using Visma.TimeTracking.EventSourcing;
using Visma.TimeTracking.EventSourcing.State;

namespace Visma.TimeTracking.Domain.Activity
{
    public sealed class ActivityState : DomainState
    {
        public string ProjectId { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public double TotalInMinutes { get; private set; }

        public void Handle(ActivityStarted @event)
        {
            Id = @event.AggregateId;
            ProjectId = @event.ProjectId;
            Description = @event.Description;
            StartDate = @event.StartDate;
        }

        public void Handle(ActivityPaused @event)
        {
            Description = @event.Description;
            EndDate = @event.EndDate;
            TotalInMinutes = @event.TotalInMinutes;
        }

        public void Handle(ActivityAdjusted @event)
        {
            Description = @event.Description;
            StartDate = @event.StartDate;
            EndDate = @event.EndDate;
            TotalInMinutes = @event.TotalInMinutes;
        }

        public override IMemento CreateMemento()
        {
            return this;
        }

        public override void SetMemento(IMemento memento)
        {
            throw new System.NotImplementedException();
        }
    }
}

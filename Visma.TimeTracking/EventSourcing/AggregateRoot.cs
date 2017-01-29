using System;
using System.Collections.Generic;
using System.Linq;
using Visma.TimeTracking.EventSourcing.Bus;
using Visma.TimeTracking.EventSourcing.Events;
using Visma.TimeTracking.EventSourcing.State;

namespace Visma.TimeTracking.EventSourcing
{
    public abstract class AggregateRoot<TState> : IEventProviderBus, IOriginator
        where TState : class, IDomainState, new()
    {
        protected readonly IList<IDomainEvent> Changes;
        protected TState State { get; }

        public string Id => State.Id;
        public long Version => State.Version;

        protected AggregateRoot(TState state = null)
        {
            Changes = new List<IDomainEvent>();
            State = state ?? new TState();
        }

        IReadOnlyCollection<IDomainEvent> IEventProviderBus.GetUncommittedChanges()
        {
            return Changes.ToList();
        }

        void IEventProviderBus.MarkChangesAsCommitted()
        {
            Changes.Clear();
        }

        void IEventProviderBus.LoadFromHistory(string streamId, IEnumerable<IDomainEvent> history)
        {
            State.LoadFromHistory(streamId, history);
        }

        IMemento IOriginator.CreateMemento()
        {
            return State.CreateMemento();
        }

        void IOriginator.SetMemento(IMemento memento)
        {
            if (memento == null) throw new ArgumentNullException(nameof(memento));

            State.SetMemento(memento);
        }

        protected void Apply(IDomainEvent @event)
        {
            State.Apply(@event);

            @event.Version = State.Version;

            Changes.Add(@event);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using Visma.TimeTracking.EventSourcing.Events;

namespace Visma.TimeTracking.EventSourcing.State
{
    public abstract class DomainState : IDomainState
    {
        public const int InitialVersion = 0;

        private long _version;

        public string Id { get; protected set; }
        public long Version => _version;

        protected DomainState(string id, long version = InitialVersion)
        {
            Id = id;
            _version = version;
        }

        protected DomainState()
        {
        }

        void IDomainState.LoadFromHistory(string streamId, IEnumerable<IDomainEvent> history)
        {
            if (history == null) throw new ArgumentNullException(nameof(history));

            Id = streamId;
            foreach (var e in history)
            {
                ((IDomainState) this).Apply(e);
            }
        }

        void IDomainState.Apply(IDomainEvent @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            ((dynamic) this).Handle((dynamic) @event);

            Interlocked.Increment(ref _version);
            @event.AggregateId = Id;
        }

        public abstract IMemento CreateMemento();

        public abstract void SetMemento(IMemento memento);
    }
}
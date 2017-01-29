using System.Collections.Generic;
using Visma.TimeTracking.EventSourcing.Events;

namespace Visma.TimeTracking.EventSourcing.State
{
    public interface IDomainState : IOriginator, IMemento
    {
        void Apply(IDomainEvent @event);
        void LoadFromHistory(string streamId, IEnumerable<IDomainEvent> history);
    }
}
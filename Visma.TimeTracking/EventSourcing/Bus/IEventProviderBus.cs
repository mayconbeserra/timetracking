using System.Collections.Generic;
using Visma.TimeTracking.EventSourcing.Events;

namespace Visma.TimeTracking.EventSourcing.Bus
{
    public interface IEventProviderBus
    {
        string Id { get; }
        long Version { get; }
        void MarkChangesAsCommitted();
        void LoadFromHistory(string streamId, IEnumerable<IDomainEvent> history);
        IReadOnlyCollection<IDomainEvent> GetUncommittedChanges();
    }
}
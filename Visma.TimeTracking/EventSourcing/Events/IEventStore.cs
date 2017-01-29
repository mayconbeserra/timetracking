using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Visma.TimeTracking.EventSourcing.Bus;

namespace Visma.TimeTracking.EventSourcing.Events
{
    public interface IEventStore
    {
        Task<IMemento> LoadMementoAsync(string streamId, CancellationToken token = default (CancellationToken));
        Task<int> SaveMementoAsync(IMemento memento, CancellationToken token = default (CancellationToken));
        Task<IReadOnlyCollection<IDomainEvent>> ReadStreamEventsAsync(string streamId, long fromVersion = 0, CancellationToken token = default (CancellationToken));
        Task<int> AppendToStreamAsync<TAggregate>(TAggregate aggregate, CancellationToken token = default (CancellationToken))
            where TAggregate: class, IEventProviderBus, IOriginator;
    }
}
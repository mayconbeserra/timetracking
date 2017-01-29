using System.Threading;
using System.Threading.Tasks;
using Visma.TimeTracking.EventSourcing;
using Visma.TimeTracking.EventSourcing.Bus;
using Visma.TimeTracking.EventSourcing.Events;

namespace Visma.TimeTracking
{
    public class DomainRepository : IDomainRepository
    {
        private readonly IEventStore _eventStore;

        public DomainRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TAggregate> Load<TAggregate>(
            string aggregateId,
            CancellationToken token = new CancellationToken()) where TAggregate :
            class, IEventProviderBus, IOriginator, new()
        {
            const long loadFromVersion = 0L;
            var events = await _eventStore.ReadStreamEventsAsync(aggregateId, loadFromVersion, token);

            if (events.Count == 0) return null;

            var aggregate = new TAggregate();
            aggregate.LoadFromHistory(aggregateId, events);

            return aggregate;
        }

        public async Task<int> Save<TAggregate>(
            TAggregate aggregate,
            CancellationToken token = new CancellationToken()) where TAggregate : class, IEventProviderBus, IOriginator
        {
            var result = await _eventStore.AppendToStreamAsync(aggregate, token);

            aggregate.MarkChangesAsCommitted();

            return result;
        }
    }
}
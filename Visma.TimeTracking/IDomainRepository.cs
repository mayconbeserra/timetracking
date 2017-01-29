using System.Threading;
using System.Threading.Tasks;
using Visma.TimeTracking.EventSourcing;
using Visma.TimeTracking.EventSourcing.Bus;

namespace Visma.TimeTracking
{
    public interface IDomainRepository
    {
        Task<TAggregate> Load<TAggregate>(string aggregateId,CancellationToken token = default (CancellationToken))
            where TAggregate : class, IEventProviderBus, IOriginator, new();

        Task<int> Save<TAggregate>(TAggregate aggregate, CancellationToken token = default (CancellationToken))
            where TAggregate : class, IEventProviderBus, IOriginator;
    }
}
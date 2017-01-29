using System.Collections.Generic;
using System.Threading.Tasks;
using Visma.TimeTracking.EventSourcing.Events;

namespace Visma.TimeTracking.EventSourcing.Bus
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(IEnumerable<TEvent> events) where TEvent : class, IDomainEvent;
    }
}
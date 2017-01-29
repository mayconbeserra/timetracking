using System.Threading.Tasks;
using Visma.TimeTracking.EventSourcing.Events;

namespace Visma.TimeTracking.EventSourcing.Handlers
{
    public interface IEventHandler<in TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
        Task Handle(TDomainEvent @event);
    }
}
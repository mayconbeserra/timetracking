using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleInjector;
using Visma.TimeTracking.EventSourcing.Bus;
using Visma.TimeTracking.EventSourcing.Events;
using Visma.TimeTracking.EventSourcing.Handlers;

namespace Visma.TimeTracking.Projections
{
    public sealed class EventProcessor : IEventPublisher
    {
        private readonly Container _container;

        public EventProcessor(Container container)
        {
            _container = container;
        }

        public async Task PublishAsync<TEvent>(IEnumerable<TEvent> events) where TEvent : class, IDomainEvent
        {
            foreach (var @event in events) await PublishAsync(@event);
        }

        private async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            var servicetype = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = _container.GetAllInstances(servicetype).ToList();

            if (handlers.Count == 0)
            {
                throw new EventHandlerNotFound($"Event {@event.GetType().Name} doesn't have registered handler");
            }

            foreach (var handler in handlers)
            {
                await ((dynamic) handler).Handle((dynamic) @event);
            }
        }
    }

    internal class EventHandlerNotFound : Exception
    {
        public EventHandlerNotFound(string s)
        {
            throw new NotImplementedException();
        }
    }
}
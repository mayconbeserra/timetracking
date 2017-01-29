using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Visma.TimeTracking.EventSourcing;
using Visma.TimeTracking.EventSourcing.Bus;
using Visma.TimeTracking.EventSourcing.Events;
using Visma.TimeTracking.EventSourcing.State;

namespace Visma.TimeTracking.Tests.Domain
{
    public abstract class AggregateTest<TAggregate, TState>
        where TAggregate : IEventProviderBus, IOriginator, new()
        where TState : class, IDomainState, new()
    {
        protected TAggregate Aggregate { get; }
        protected TState State => Aggregate.CreateMemento() as TState;

        protected IDomainEvent[] UncommittedChanges => Aggregate.GetUncommittedChanges().ToArray();

        protected AggregateTest()
        {
            Aggregate = new TAggregate();
        }

        protected void LoadFromHistory(string stream, IEnumerable<IDomainEvent> history)
        {
            Aggregate.LoadFromHistory(stream, history);
        }

        protected void Log(object obj)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            System.Console.WriteLine("{0}", JsonConvert.SerializeObject(State, settings));
        }
    }
}
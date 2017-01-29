using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Visma.TimeTracking.EventSourcing.Bus;
using Visma.TimeTracking.EventSourcing.Events;
using Visma.TimeTracking.EventSourcing.Models;
using Visma.TimeTracking.Extensions;

namespace Visma.TimeTracking.EventSourcing.Db
{
    public sealed class EventStore : IEventStore
    {
        private readonly JsonSerializerSettings _jsonSettings;
        private EventStoreDbContext Db { get; }
        private IEventPublisher Publisher { get; }

        public EventStore(EventStoreDbContext db, IEventPublisher publisher)
        {
            Db = db;
            Publisher = publisher;
            _jsonSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
        }

        public async Task<IReadOnlyCollection<IDomainEvent>> ReadStreamEventsAsync(string streamId,
            long fromVersion = 0, CancellationToken token = default(CancellationToken))
        {
            var events = await Db
                .Events
                .Where(e => e.StreamId == streamId && e.Version >= fromVersion)
                .Select(e =>
                    (IDomainEvent) JsonConvert.DeserializeObject(e.Payload, _jsonSettings))
                .ToListAsync(cancellationToken: token);

            return events.AsReadOnly();
        }

        public async Task<int> AppendToStreamAsync<TAggregate>(TAggregate aggregate, CancellationToken token)
            where TAggregate : class, IEventProviderBus, IOriginator
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));

            var events = aggregate.GetUncommittedChanges();
            var expectedVersion = aggregate.Version - events.Count;

            var stream = await Db
                .Streams
                .FirstOrDefaultAsync(e => e.Id == aggregate.Id, token);
            if (stream != null && stream.Version != expectedVersion)
            {
                throw new ConcurrencyException(aggregate.Id);
            }

            if (stream == null)
            {
                stream = new Stream()
                {
                    Id = aggregate.Id,
                    Version = aggregate.Version
                };
                Db.Streams.Add(stream);
            }
            else stream.Version = aggregate.Version;

            var entities = events.ToEventEntities(serializerSettings: _jsonSettings);
            Db.Events.AddRange(entities);

            using (var tr = Db.Database.BeginTransaction())
            {
                await Publisher.PublishAsync(events);
                var updated = await Db.SaveChangesAsync(token);

                tr.Commit();

                return updated;
            }
        }

        public Task<IMemento> LoadMementoAsync(string streamId, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveMementoAsync(IMemento memento, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }

    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string aggregateId)
        {
            throw new NotImplementedException(nameof(aggregateId));
        }
    }
}
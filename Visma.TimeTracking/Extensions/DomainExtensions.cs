using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Visma.TimeTracking.EventSourcing.Events;
using Visma.TimeTracking.EventSourcing.Models;

namespace Visma.TimeTracking.Extensions
{
    public static class DomainExtensions
    {
        public static IEnumerable<Event> ToEventEntities(this IReadOnlyCollection<IDomainEvent> events,
            JsonSerializerSettings serializerSettings)
        {
            if (events == null) throw new ArgumentNullException(nameof (events));
            if (serializerSettings == null) throw new ArgumentNullException(nameof (serializerSettings));

            return events.Select(e => new Event
            {
                Id = e.Id,
                StreamId = e.AggregateId,
                CommitId = e.CorrelationId,
                Type = e.Type,
                Payload = JsonConvert.SerializeObject(e, serializerSettings),
                Version = e.Version,
                CreatedAt = e.CreatedAt,
                CreatorId = e.CreatorId
            }).ToList();
        }
    }
}
namespace Post.Cmd.Infrastructure.Stores;

using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Domain.Aggregates;

public class EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer) : IEventStore
{
	public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
	{
		List<EventModel> eventStream = await eventStoreRepository.FindByAggregateId(aggregateId);

		if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)// ^1 means the last element
		{
			throw new ConcurrencyException();
		}

		int version = expectedVersion;

		foreach (var @event in events)
		{
			version++;
			string eventType = @event.GetType().Name;

			var eventModel = new EventModel{
				TimeStamp = DateTime.Now,
				AggregateIdentifier = aggregateId,
				AggregateType = nameof(PostAggregate),
				Version = version,
				EventType = eventType,
				EventData = @event
			};
			@event.Version = version;

			await eventStoreRepository.SaveAsync(eventModel);

			string topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
			await eventProducer.ProduceAsync(topic!, @event);
		}
	}

	public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
	{
		List<EventModel> eventStream = await eventStoreRepository.FindByAggregateId(aggregateId);

		if (eventStream == null || eventStream.Count == 0)
		{
			throw new AggregateNotFoundException("Incorrect post Id provided! "+aggregateId);
		}

		return eventStream.OrderBy(x => x.Version)
			.Select(x => x.EventData)
			.ToList();
	}
}

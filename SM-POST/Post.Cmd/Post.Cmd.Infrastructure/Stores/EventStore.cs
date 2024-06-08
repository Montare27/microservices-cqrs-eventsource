namespace Post.Cmd.Infrastructure.Stores;

using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Domain.Aggregates;
using System.Data;

public class EventStore(IEventStoreRepository eventStoreRepository) : IEventStore
{
	private readonly IEventStoreRepository _eventStoreRepository = eventStoreRepository;

	public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
	{
		List<EventModel> eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

		if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)// ^1 means the last element
			throw new ConcurrencyException();

		int version = expectedVersion;
		
		foreach (var @event in events)
		{
			version++;
			var eventType = @event.GetType().Name;
			
			var eventModel = new EventModel{
				TimeStamp = DateTime.Now,
				AggregateIdentifier = aggregateId,
				AggregateType = nameof(PostAggregate),
				Version = version,
				EventType = eventType,
				EventData = @event
			};
			@event.Version = version;

			await _eventStoreRepository.SaveAsync(eventModel);
		}
	}

	public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
	{
		List<EventModel> eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

		if (eventStream == null || eventStream.Count == 0)
			throw new ArgumentNotFoundException("Incorrect post Id provided! "+ aggregateId);

		return eventStream.OrderBy(x => x.Version)
			.Select(x => x.EventData)
			.ToList();
	}
}

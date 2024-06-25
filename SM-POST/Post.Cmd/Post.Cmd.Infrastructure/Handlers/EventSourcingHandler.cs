namespace Post.Cmd.Infrastructure.Handlers;

using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Domain.Aggregates;

public class EventSourcingHandler(
	IEventStore eventStore,
	IEventProducer eventProducer
) : IEventSourcingHandler<PostAggregate>
{
	public async Task SaveAsync(AggregateRoot aggregateRoot)
	{
		await eventStore.SaveEventsAsync(aggregateRoot.Id, aggregateRoot.GetUncommittedChanges(), aggregateRoot.Version);
		aggregateRoot.MarkChangesAsCommitted();
	}

	public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
	{
		var aggregate = new PostAggregate();
		List<BaseEvent> events = await eventStore.GetEventsAsync(aggregateId);

		if (events == null || events.Count == 0)
		{
			return aggregate;
		}

		aggregate.ReplayEvents(events);
		aggregate.Version = events.Select(x => x.Version).Max();
		return aggregate;
	}

	public async Task RepublishEventsAsync()
	{
		var aggregateIds = await eventStore.GetAggregateIdsAsync();
		
		if(aggregateIds.Count == 0) return;
		
		foreach (var aggregateId in aggregateIds)
		{
			var aggregate = await GetByIdAsync(aggregateId);
			if(aggregate is null || !aggregate.Active) continue;
			
			var events = await eventStore.GetEventsAsync(aggregateId);
			
			foreach (var baseEvent in events)
			{
				var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
				await eventProducer.ProduceAsync(topic!, baseEvent);
			}
		}
	}
}

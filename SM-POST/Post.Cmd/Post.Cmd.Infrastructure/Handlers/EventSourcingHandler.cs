namespace Post.Cmd.Infrastructure.Handlers;

using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Domain.Aggregates;

public class EventSourcingHandler(IEventStore eventStore) : IEventSourcingHandler<PostAggregate>
{
	public async Task SaveAsync(AggregateRoot aggregateRoot)
	{
		await eventStore.SaveEventsAsync(aggregateRoot.Id, aggregateRoot.GetUncommittedChanges(), aggregateRoot.Version);
		aggregateRoot.MarkChangesAsCommitted();
	}

	public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
	{
		var aggregate = new PostAggregate();
		var events = await eventStore.GetEventsAsync(aggregateId);

		if (events == null || events.Count == 0) return aggregate;

		aggregate.ReplayEvents(events);
		aggregate.Version = events.Select(x => x.Version).Max();
		return aggregate;
	}
}

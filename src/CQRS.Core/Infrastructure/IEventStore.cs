namespace CQRS.Core.Infrastructure;

using Events;

public interface IEventStore
{
	Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);

	Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
	
	Task<List<Guid>> GetAggregateIdsAsync();
}

namespace CQRS.Core.Handlers;

using Domain;

public interface IEventSourcingHandler<T>
{
	Task SaveAsync(AggregateRoot aggregateRoot);

	Task<T> GetByIdAsync(Guid aggregateId);
	
	Task RepublishEventsAsync();
}

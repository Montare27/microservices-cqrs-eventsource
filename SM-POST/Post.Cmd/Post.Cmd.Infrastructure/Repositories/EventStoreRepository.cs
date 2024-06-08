namespace Post.Cmd.Infrastructure.Repositories;

using Config;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class EventStoreRepository : IEventStoreRepository
{
	private readonly IMongoCollection<EventModel> _eventStoreCollection;

	public EventStoreRepository(IOptions<MongoDbConfig> config)
	{
		var mongoClient = new MongoClient(config.Value.ConnectionString);
		var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);
		
		_eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
	}
	
	public async Task SaveAsync(EventModel @event)
	{
		await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
	}

	public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
	{
		return await _eventStoreCollection
			.Find(x => x.AggregateIdentifier.Equals(aggregateId))
			.ToListAsync()
			.ConfigureAwait(false);  
		// avoids forcing the callback to be invoked on the original context or scheduler.
		// such as improving performance and avoiding deadlocks
		// Deadlock is when 2+ threads are blocked, each waiting for a resource that is held by another thread, leading to a program's halt.
	}
}

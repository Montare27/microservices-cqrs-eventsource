namespace Post.Cmd.Infrastructure.Producers;

using CQRS.Core.Events;
using CQRS.Core.Producers;

public class EventProducer : IEventProducer
{

	public Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
	{
		throw new NotImplementedException();
	}
}

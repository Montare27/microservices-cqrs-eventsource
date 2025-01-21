namespace CQRS.Core.Producers;

using Events;

public interface IEventProducer
{
	Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
}

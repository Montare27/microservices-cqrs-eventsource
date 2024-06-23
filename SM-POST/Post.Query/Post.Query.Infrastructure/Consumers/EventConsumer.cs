namespace Post.Query.Infrastructure.Consumers;

using Confluent.Kafka;
using Converters;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Handlers;
using Microsoft.Extensions.Options;
using System.Text.Json;

public class EventConsumer(
	IOptions<ConsumerConfig> config, 
	IEventHandler eventHandler
) : IEventConsumer
{
	private readonly ConsumerConfig _config = config.Value;
	
	public void Consume(string topic)
	{
		using var consumer = new ConsumerBuilder<string, string>(_config)
			.SetKeyDeserializer(Deserializers.Utf8)
			.SetValueDeserializer(Deserializers.Utf8)
			.Build();
		
		consumer.Subscribe(topic);
		var options = new JsonSerializerOptions{Converters ={new EventJsonConverter()}};
		
		while (true)
		{
			var consumerResult = consumer.Consume();
			
			if(consumerResult?.Message == null) continue;
			
			// create a json options with our custom converter
			var @event = JsonSerializer.Deserialize<BaseEvent>(consumerResult.Message.Value, options);
			var handlerMethod = eventHandler.GetType().GetMethod("On", [@event!.GetType()]);

			if (handlerMethod == null)
				throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");

			handlerMethod.Invoke(eventHandler, [@event]);
			
			// commits an offset for the message. Hence, next time it will start from the next message
			consumer.Commit(consumerResult);  
		}
	}
}

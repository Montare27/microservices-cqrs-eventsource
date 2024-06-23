namespace Post.Query.Infrastructure.Consumers;

using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class ConsumerHostedService(
	ILogger<ConsumerHostedService> logger,
	IServiceProvider serviceProvider  
) : IHostedService
{

	public Task StartAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Event Consumer service running.");

		using(var scope = serviceProvider.CreateScope())
		{
			var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
			var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

			Task.Run(() => eventConsumer.Consume(topic!), cancellationToken);
		}
		
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Event Consumer service stopped.");
		return Task.CompletedTask;
	}
}

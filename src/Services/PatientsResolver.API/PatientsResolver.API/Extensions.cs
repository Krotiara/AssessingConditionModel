using ASMLib.EventBus;
using ASMLib.EventBus.RabbitMQ;
using PatientsResolver.API.Service;
using RabbitMQ.Client;

namespace PatientsResolver.API
{
	public static class Extensions
	{
		private static void AddRabbitMQEventBus(this IServiceCollection services, string connectionUrl, string brokerName, string queueName, int timeoutBeforeReconnecting = 15)
		{
			services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionManager>();
			services.AddSingleton<IPersistentConnection, RabbitMQPersistentConnection>(factory =>
			{
				var connectionFactory = new ConnectionFactory
				{
					Uri = new Uri(connectionUrl),
					DispatchConsumersAsync = true,
				};

				var logger = factory.GetService<ILogger<RabbitMQPersistentConnection>>();
				return new RabbitMQPersistentConnection(connectionFactory, logger, timeoutBeforeReconnecting);
			});

			services.AddSingleton<IEventBus, RabbitMQEventBus>(factory =>
			{
				var persistentConnection = factory.GetService<IPersistentConnection>();
				var subscriptionManager = factory.GetService<IEventBusSubscriptionManager>();
				var logger = factory.GetService<ILogger<RabbitMQEventBus>>();

				return new RabbitMQEventBus(persistentConnection, subscriptionManager, factory, logger, brokerName, queueName);
			});
		}


		public static void AddRabbitMQEventBus(this IServiceCollection services, IConfiguration conf)
        {
			var rabbitMQSection = conf.GetSection("RabbitMQSettings");
			if (rabbitMQSection == null)
				return;

			services.AddRabbitMQEventBus
			(
				connectionUrl: rabbitMQSection["ConnectionUrl"],
				brokerName: rabbitMQSection["BrokerName"],
				queueName: rabbitMQSection["QueueName"],
				timeoutBeforeReconnecting: 15
			);

			services.AddTransient<MessageSentEventHandler>();
		}
	}
}

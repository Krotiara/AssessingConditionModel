using System;
using RabbitMQ.Client;


namespace ASMLib.EventBus.RabbitMQ
{
    public interface IPersistentConnection
	{
		event EventHandler OnReconnectedAfterConnectionFailure;
		bool IsConnected { get; }

		bool TryConnect();
		IModel CreateModel();
	}
}

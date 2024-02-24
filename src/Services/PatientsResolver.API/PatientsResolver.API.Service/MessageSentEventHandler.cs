using ASMLib.EventBus;
using ASMLib.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service
{
    public class MessageSentEventHandler : IEventHandler<MessageSentEvent>
	{
		private readonly ILogger<MessageSentEvent> _logger;

		public MessageSentEventHandler(ILogger<MessageSentEvent> logger)
		{
			_logger = logger;
		}

		public Task HandleAsync(MessageSentEvent @event)
		{
			// Here you handle what happens when you receive an event of this type from the event bus.
			_logger.LogInformation(@event.ToString());
			return Task.CompletedTask;
		}
	}
}

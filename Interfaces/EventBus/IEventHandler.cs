using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.EventBus
{
	public interface IEventHandler<in TEvent>
		where TEvent : Event
	{
		Task HandleAsync(TEvent @event);
	}
}

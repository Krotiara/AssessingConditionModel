using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{
    public class AddAgingStateCommand: IRequest<AgingState>
    {
        public AgingState AgingStateToAdd { get; set; }
    }
}

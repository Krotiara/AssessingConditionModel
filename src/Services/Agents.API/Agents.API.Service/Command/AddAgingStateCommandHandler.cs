using Agents.API.Data.Repository;
using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{
    public class AddAgingStateCommandHandler : IRequestHandler<AddAgingStateCommand, AgingState>
    {
        private readonly IAgingStatesRepository agingStatesRepository;


        public AddAgingStateCommandHandler(IAgingStatesRepository agingStatesRepository)
        {
            this.agingStatesRepository = agingStatesRepository;
        }

        public async Task<AgingState> Handle(AddAgingStateCommand request, CancellationToken cancellationToken)
        {
            return await agingStatesRepository.AddState(request.AgingStateToAdd, false); //TODO try catch
        }
    }
}

using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{

    public class UpdateInfluenceCommand : IRequest<Influence>
    {
        public Influence Influence { get; }

        public UpdateInfluenceCommand(Influence influence)
        {
            Influence = influence;
        }

    }

    public class UpdateInfluenceCommandHandler : IRequestHandler<UpdateInfluenceCommand, Influence>
    {
        private IInfluenceRepository influenceRepository;

        public UpdateInfluenceCommandHandler(IInfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }

        public async Task<Influence> Handle(UpdateInfluenceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await influenceRepository.UpdateInfluence(request.Influence, cancellationToken);
            }
            catch(Exception ex)
            {
                throw new UpdateInfluenceException("", ex);
            }
        }
    }
}

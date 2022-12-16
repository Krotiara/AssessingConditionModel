using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{

    public class AddPatientInfluenceCommand : IRequest<bool>
    {
        public Influence Influence { get; }

        public AddPatientInfluenceCommand(Influence influence)
        {
            Influence = influence;
        }
    }

    public class AddPatientInfluenceCommandHandler : IRequestHandler<AddPatientInfluenceCommand, bool>
    {
        private readonly IInfluenceRepository influenceRepository;

        public AddPatientInfluenceCommandHandler(IInfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }

        public async Task<bool> Handle(AddPatientInfluenceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await influenceRepository.AddPatientInluence(request.Influence, cancellationToken);
            }
            catch(Exception ex)
            {
                throw new AddInfluenceException("Error adding influence, see inner exception", ex);
            }
        }
    }
}

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

    public class DeleteInfluenceCommand: IRequest<bool>
    {
        public int InfluenceId { get; }

        public DeleteInfluenceCommand(int influenceId)
        {
            InfluenceId = influenceId;
        }

    }

    public class DeleteInfluenceCommandHandler : IRequestHandler<DeleteInfluenceCommand, bool>
    {
        private readonly IInfluenceRepository influenceRepository;

        public DeleteInfluenceCommandHandler(IInfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }

        public async Task<bool> Handle(DeleteInfluenceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await influenceRepository.DeleteInfluence(request.InfluenceId, cancellationToken);
            }
            catch(Exception ex)
            {
                throw new DeleteInfluenceException("", ex);
            }
        }
    }
}

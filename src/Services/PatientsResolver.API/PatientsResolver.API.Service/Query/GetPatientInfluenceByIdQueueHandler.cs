using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Query
{
    public class GetPatientInfluenceByIdQueueHandler : IRequestHandler<GetPatientInfluenceByIdQueue, Influence>
    {
        private readonly IInfluenceRepository influenceRepository;

        public GetPatientInfluenceByIdQueueHandler(IInfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }


        public async Task<Influence> Handle(GetPatientInfluenceByIdQueue request, CancellationToken cancellationToken)
        {
            Influence? inf = await influenceRepository.GetPatientInfluence(request.InfluenceId);
            if (inf == null)
                throw new InfluenceNotFoundException($"Не найдено воздействие с id={request.InfluenceId}");
            else
                return inf;
        }
    }
}

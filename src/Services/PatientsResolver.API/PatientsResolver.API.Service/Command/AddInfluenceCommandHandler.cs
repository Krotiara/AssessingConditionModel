using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class AddInfluenceCommandHandler : IRequestHandler<AddInfluenceCommand, bool>
    {
        private readonly InfluenceRepository influenceRepository;

        public AddInfluenceCommandHandler(InfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }

        public async Task<bool> Handle(AddInfluenceCommand request, CancellationToken cancellationToken)
        {
            bool isInfluenceExist = influenceRepository.GetAll().FirstOrDefault(x =>
            x.PatientId == request.Influence.PatientId &&
            x.InfluenceType == request.Influence.InfluenceType &&
            x.MedicineName == request.Influence.MedicineName &&
            x.StartTimestamp == request.Influence.StartTimestamp &&
            x.EndTimestamp == request.Influence.EndTimestamp) != null; //TODO Засунуть в equalityComparer

            if (isInfluenceExist)
                throw new AddInfluenceException("Influence already exist.");

            try
            {
                await influenceRepository.AddAsync(request.Influence);
                return true;
            }
            catch(Exception ex)
            {
                throw new AddInfluenceException("Add influence error", ex);
            }
        }
    }
}

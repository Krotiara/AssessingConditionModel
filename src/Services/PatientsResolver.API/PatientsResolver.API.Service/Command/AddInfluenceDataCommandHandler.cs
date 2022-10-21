using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Service.Command
{
    public class AddInfluenceDataCommandHandler : IRequestHandler<AddInfluenceDataCommand, List<Influence>>
    {

        private readonly IInfluenceRepository influenceRepository;

        public AddInfluenceDataCommandHandler(IInfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }


        public async Task<List<Influence>> Handle(AddInfluenceDataCommand request, CancellationToken cancellationToken)
        {
            foreach (Influence p in request.Data)
                try
                {
                    await influenceRepository.AddPatientInluence(p, cancellationToken);
                }
                catch(Exception ex)
                {
                    //TODO log
                    continue;
                }
            return request.Data;
        }
    }
}

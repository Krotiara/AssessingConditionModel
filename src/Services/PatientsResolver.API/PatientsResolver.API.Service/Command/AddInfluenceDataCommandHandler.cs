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
            List<Influence> addedData = new List<Influence>();
            foreach (Influence p in request.Data)
                try
                {
                    bool isAdd = await influenceRepository.AddPatientInluence(p, cancellationToken);
                    if(isAdd)
                        addedData.Add(p);
                }
                catch(Exception ex)
                {
                    //TODO log
                    continue;
                }
            return addedData;
        }
    }
}

using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Exceptions;

namespace PatientsResolver.API.Service.Command
{

    public class AddInfluenceDataCommand : IRequest<List<Influence>>
    {
        public List<Influence> Data { get; set; }
    }

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
            List<string> exceptions = new List<string>();
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
                    exceptions.Add($"Add influence exception: {ex.Message}");
                    continue;
                }
            if (exceptions.Count > 0)
                throw new AddInfluenceRangeException(string.Join("\n", exceptions));
            else
                return addedData;
        }
    }
}

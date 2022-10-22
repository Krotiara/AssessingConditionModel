using MediatR;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Service.Command
{
    public class AddInfluenceDataCommand: IRequest<List<Influence>>
    {
        public List<Influence> Data { get; set; }
    }
}

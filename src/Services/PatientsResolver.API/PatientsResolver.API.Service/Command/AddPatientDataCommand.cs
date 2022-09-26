using MediatR;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Service.Command
{
    public class AddPatientDataCommand: IRequest<List<PatientData>>
    {
        public List<PatientData> Data { get; set; }
    }
}

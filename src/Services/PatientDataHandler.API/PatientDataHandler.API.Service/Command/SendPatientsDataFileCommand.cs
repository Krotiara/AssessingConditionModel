using MediatR;
using PatientDataHandler.API.Entities;


namespace PatientDataHandler.API.Service.Command
{
    public class SendPatientsDataFileCommand: IRequest
    {
        public FileData Data { get; set; }
    }
}

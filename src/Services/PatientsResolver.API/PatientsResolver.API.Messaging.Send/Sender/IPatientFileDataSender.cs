using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Messaging.Send.Sender
{
    public interface IPatientFileDataSender
    {
        bool SendPatientsFileData(FileData data);
    }
}

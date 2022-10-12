using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API_Messaging.Send.Sender
{
    public interface IPatientsDataSender
    {
        void SendPatientsData(IList<IPatientData<IPatientParameter, IPatient, IInfluence>> data);
    }
}

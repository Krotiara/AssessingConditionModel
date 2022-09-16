using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Messaging.Send.Sender
{
    public interface IPatientDataUpdateSender
    {
        void SendPatientData(IPatientData data);
    }
}

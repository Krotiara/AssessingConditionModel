﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Messaging.Send.Sender
{
    public interface IUpdatePatientsSender
    {
        void SendUpdatePatientsInfo(IUpdatePatientsDataInfo updatePatientsInfo);
    }
}

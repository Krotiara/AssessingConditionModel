﻿using MediatR;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Query
{
    public class GetPatientQuery: IRequest<Patient>
    {
        public int PatientId { get; set; }
    }
}

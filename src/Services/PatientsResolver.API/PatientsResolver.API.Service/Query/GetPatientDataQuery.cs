﻿using MediatR;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Query
{
    public class GetPatientDataQuery: IRequest<List<PatientData>>
    {
        public GetPatientDataQuery(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            PatientId = patientId;
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
        }

        public GetPatientDataQuery(int patientId):
            this(patientId, DateTime.MinValue, DateTime.MaxValue)
        {

        }

        public int PatientId { get; set; }

        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }
    }
}

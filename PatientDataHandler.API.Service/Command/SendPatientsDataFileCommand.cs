﻿using Interfaces;
using MediatR;
using PatientDataHandler.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Service.Command
{
    public class SendPatientsDataFileCommand: IRequest
    {
        public FileData Data { get; set; }
    }
}
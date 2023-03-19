﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Entities
{
    public class FileData : IFileData
    {

        public string MedicalOrganization { get; set; }
        public byte[] RawData { get; set; }
    }
}

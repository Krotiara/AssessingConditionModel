﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IFileData
    {
        public string MedicalOrganization { get; set; }

        public byte[] RawData { get; set; }
    }
}

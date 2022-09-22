using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class FileData : IFileData
    {
        public byte[] RawData { get; set; }
    }
}

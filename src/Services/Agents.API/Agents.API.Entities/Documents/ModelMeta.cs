using Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Documents
{
    public class ModelMeta: Document
    {
        public string StorageId { get; set; }


        public string Version { get; set; }


        public string FileName { get; set; }


        public int Accuracy { get; set; }


        public int InputCount { get; set; }


        public int OutputCount { get; set; }


        public string ParamsNames { get; set; }


        public string Description { get; set; }

        public List<string> ParamsNamesList =>
            ParamsNames.Split(",").Select(x => x.Trim()).ToList();
    }
}

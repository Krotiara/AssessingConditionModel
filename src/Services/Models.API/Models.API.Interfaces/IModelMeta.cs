using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Interfaces
{
    public interface IModelMeta
    {
        public string Id { get; set; }

        public string File { get; set; }

        public double Accuracy { get; set; }

        public string Version { get; set; }

        public int InputParamsCount { get; set; }

        public int OutputParamsCount { get; set; }

        public string[] ParamsNames { get; set; }
    }
}

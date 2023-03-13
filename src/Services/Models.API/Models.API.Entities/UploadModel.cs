using Models.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Entities
{
    public class UploadModel : IModelMeta
    {
        public string StorageId { get ; set ; }
        public string Name { get ; set ; }
        public double Accuracy { get ; set ; }
        public double Version { get ; set ; }
        public int InputParamsCount { get ; set ; }
        public int OutputParamsCount { get ; set ; }
        public string[] ParamsNames { get ; set ; }

        public string File { get; set; }
    }
}

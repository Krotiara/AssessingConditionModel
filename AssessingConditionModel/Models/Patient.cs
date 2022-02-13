using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public class Patient
    {
        public string Name { get; set; }

        public Parameters<ClinicalParams> ClinicalParams { get; private set; }

        public Parameters<NormParams> ParamsNorms { get; private set; }

        public Patient(string name)
        {
            Name = name;
            ClinicalParams = new Parameters<ClinicalParams>();
            ParamsNorms = new Parameters<NormParams>();
        }
    }
}

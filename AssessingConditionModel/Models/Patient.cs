using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public class Patient
    {
        public string Name { get; set; }

        public Dictionary<ParamsNames, string> Parameters { get; private set; }

        public Dictionary<ParamsNames, string> ParametersNorms { get; private set; }

        public Patient(string name)
        {
            Name = name;
            Parameters = new Dictionary<ParamsNames, string>();
            ParametersNorms = new Dictionary<ParamsNames, string>();
        }


        public T GetParamValue<T>(ParamsNames parameterName)
        {
            return  Parameters[parameterName].ParseTo<T>();
        }


        public T GetParamNormValue<T>(ParamsNames parameterName)
        {
            return ParametersNorms[parameterName].ParseTo<T>();
        } 
    }
}

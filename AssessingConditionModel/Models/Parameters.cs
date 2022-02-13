using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public class Parameters<T>
    {
        public DateTime Date { get; set; }

        private readonly Dictionary<T, string> parameters;


        public Parameters()
        {
            parameters = new Dictionary<T, string>();
        }


        public void SetParam(T paramType, string value)
        {
            parameters[paramType] = value;
        }


        public T1 GetParam<T1>(T paramType)
        {
            return parameters[paramType].ParseTo<T1>();
        }      
    }
}

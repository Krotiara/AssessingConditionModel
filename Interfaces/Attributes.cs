using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NoAllowToSelectAttribute : Attribute
    {
    }


    public class ParamDescriptionAttribute : Attribute
    {
        public string[] Descriptions { get; set; }

        public HashSet<string> DescriptionsSet => new HashSet<string>(Descriptions);

    }


    [AttributeUsage(AttributeTargets.Field)]
    public class ParamValueTypeAttribute: Attribute
    {
        public Type ValueType { get; set; }
    }
}

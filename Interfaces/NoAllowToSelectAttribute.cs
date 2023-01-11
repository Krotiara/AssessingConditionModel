using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NoAllowToSelectAttribute: Attribute
    {
    }
}

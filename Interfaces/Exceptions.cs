using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib
{
    public class EntityAlreadyExistException : Exception
    {
        public EntityAlreadyExistException(string message) : base(message) { }
    }
}

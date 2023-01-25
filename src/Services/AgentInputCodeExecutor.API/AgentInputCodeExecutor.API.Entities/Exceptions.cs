using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class ParseCodeLineException: Exception
    {
        public ParseCodeLineException(string message): base(message)
        {

        }
    }
}

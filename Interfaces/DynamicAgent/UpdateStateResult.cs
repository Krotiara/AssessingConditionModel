using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public enum UpdateStateStatus
    {
        Success = 0,
        Error = 1
    }


    public class UpdateStateResult
    {
        public UpdateStateStatus Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}

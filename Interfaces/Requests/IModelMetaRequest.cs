using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Requests
{
    public interface IModelMetaRequest
    {
        public string Id { get; set; }

        public string Version { get; set; }
    }
}

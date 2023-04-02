using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPredictRequest
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public float[] Input { get; set; }
    }
}

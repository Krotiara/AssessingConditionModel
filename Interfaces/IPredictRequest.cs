using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPredictRequest
    {
        public string ModelId { get; set; }

        public float[] InputArgs { get; set; }
    }
}

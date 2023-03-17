using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Entities
{
    public class PredictRequest : IPredictRequest
    {
        public PredictRequest() { }

        public string ModelId { get; set; }
        public float[] InputArgs { get; set; }
    }
}

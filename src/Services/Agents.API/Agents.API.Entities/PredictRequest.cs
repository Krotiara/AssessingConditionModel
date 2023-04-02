using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class PredictRequest : IPredictRequest
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public float[] Input { get; set; }
    }
}

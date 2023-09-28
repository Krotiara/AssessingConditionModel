using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class PredictRequest
    {
        public string Id { get; set; }

        public double[] Input { get; set; }
    }
}

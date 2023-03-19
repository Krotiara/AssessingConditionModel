using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Interfaces
{
    public interface IPredictor
    {
        public double[] Predict(string modelPath, double[] args, int[] dimensions);
    }
}

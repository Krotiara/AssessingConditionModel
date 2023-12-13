using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests.Responce
{
    public enum PredictStatus : byte
    {
        Success = 0,
        WaitModelDownloading = 1,
        Error = 2
    }

    public class PredictResponce
    {
        public float[] Predictions { get; set; }

        public PredictStatus Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}

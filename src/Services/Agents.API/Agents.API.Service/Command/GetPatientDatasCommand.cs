using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{
    public class GetPatientDatasCommand: IRequest<IList<IPatientData<IPatientParameter,IPatient,IInfluence>>>
    {
        public int PatientId { get; set; }

        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }
    }
}

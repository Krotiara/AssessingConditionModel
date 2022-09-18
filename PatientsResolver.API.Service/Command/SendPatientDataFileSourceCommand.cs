using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class SendPatientDataFileSourceCommand: IRequest
    {
        public Stream Stream { get; set; }
    }
}

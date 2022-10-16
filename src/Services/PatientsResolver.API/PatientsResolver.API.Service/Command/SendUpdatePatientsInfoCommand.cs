using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class SendUpdatePatientsInfoCommand: IRequest
    {
        public IUpdatePatientsInfo UpdatePatientsInfo { get; set; }
    }
}

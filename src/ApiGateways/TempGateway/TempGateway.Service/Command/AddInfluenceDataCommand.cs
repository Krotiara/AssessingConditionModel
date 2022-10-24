using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempGateway.Entities;

namespace TempGateway.Service.Command
{
    public class AddInfluenceDataCommand: IRequest
    {
        public string FilePath { get; set; }
    }
}

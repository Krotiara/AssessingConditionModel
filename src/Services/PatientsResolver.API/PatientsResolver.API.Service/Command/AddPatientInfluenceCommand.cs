using MediatR;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class AddPatientInfluenceCommand: IRequest<bool>
    {
        public Influence Influence { get; }

        public AddPatientInfluenceCommand(Influence influence)
        {
            Influence = influence;
        }
    }
}

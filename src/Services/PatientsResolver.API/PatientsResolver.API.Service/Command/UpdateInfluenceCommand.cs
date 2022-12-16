using MediatR;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class UpdateInfluenceCommand : IRequest<Influence>
    {
        public Influence Influence { get; }

        public UpdateInfluenceCommand(Influence influence)
        {
            Influence = influence;
        }

    }
}

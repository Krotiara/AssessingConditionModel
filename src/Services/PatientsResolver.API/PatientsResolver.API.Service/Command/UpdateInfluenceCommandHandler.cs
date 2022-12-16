﻿using MediatR;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Command
{
    public class UpdateInfluenceCommandHandler : IRequestHandler<UpdateInfluenceCommand, Influence>
    {
        private IInfluenceRepository influenceRepository;

        public UpdateInfluenceCommandHandler(IInfluenceRepository influenceRepository)
        {
            this.influenceRepository = influenceRepository;
        }

        public async Task<Influence> Handle(UpdateInfluenceCommand request, CancellationToken cancellationToken)
        {
            return await influenceRepository.UpdateInfluence(request.Influence, cancellationToken);
        }
    }
}

﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Data
{
    public class ModelsMetaStore
    {
        private readonly ModelsMetaDbContext _dbContext;

        public ModelsMetaStore(ModelsMetaDbContext dbContext, ILogger<ModelsMetaStore> logger)
        {
            _dbContext = dbContext;
        }


        public async Task Insert(ModelMeta modelMeta)
        {
#warning Выскакивает эксепшен connection refused
            await _dbContext.ModelMetas.AddAsync(modelMeta);
            await _dbContext.SaveChangesAsync();
        }

    }
}

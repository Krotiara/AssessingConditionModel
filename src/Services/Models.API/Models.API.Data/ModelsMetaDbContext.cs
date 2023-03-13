using Microsoft.EntityFrameworkCore;
using Models.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Data
{
    public class ModelsMetaDbContext: DbContext
    {
        public DbSet<ModelMeta> ModelMetas { get; set; }

        public ModelsMetaDbContext() : base() { }

        public ModelsMetaDbContext(DbContextOptions<ModelsMetaDbContext> options) : base(options) {}
    }
}

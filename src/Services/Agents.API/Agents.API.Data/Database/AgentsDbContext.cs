using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agents.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agents.API.Data.Database
{
    public class AgentsDbContext: DbContext
    {
        public virtual DbSet<AgentPatient> AgentPatients { get; set; }

        public AgentsDbContext() : base() { }

        public AgentsDbContext(DbContextOptions<AgentsDbContext> options) : base(options) { }
    }
}

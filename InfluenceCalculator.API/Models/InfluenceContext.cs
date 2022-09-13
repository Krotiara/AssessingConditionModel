using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfluenceCalculator.API.Models
{
    public class InfluenceContext: DbContext
    {

        public InfluenceContext(): base()
        { 
        }

        public InfluenceContext(DbContextOptions<InfluenceContext> options): base(options)
        {

        }

        public virtual DbSet<InfluenceResult> InfluenceResults { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IInfluenceResult>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }

    }
}

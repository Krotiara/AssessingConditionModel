using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfluenceCalculator.API.Models
{
    public class InfluenceContext: DbContext
    {

        public InfluenceContext(DbContextOptions<InfluenceContext> options): base(options)
        {

        }

        public virtual DbSet<IInfluenceResult> InfluenceResults { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IInfluenceResult>()
                .HasOne(x => x.Influence)
                .WithOne()
                .IsRequired();

            modelBuilder.Entity<IInfluenceResult>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }

    }
}

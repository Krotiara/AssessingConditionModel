using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PatientDataHandler.API.Models
{
    public class PatientsDataDbContext: DbContext
    {

        public virtual DbSet<PatientParameter> PatientsParameters { get; set; }

        public virtual DbSet<PatientData> PatientDatas { get; set; }


        public PatientsDataDbContext():base()
        {

        }

        public PatientsDataDbContext(DbContextOptions<PatientsDataDbContext> options):base(options)
        {

        }
    }
}

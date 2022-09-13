using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PatientDataHandler.API.Models
{
    public class PatientsDataDbContext: DbContext
    {

        public virtual DbSet<IPatientData> PatientDatas { get; set; }

        public PatientsDataDbContext():base()
        {

        }

        public PatientsDataDbContext(DbContextOptions<PatientsDataDbContext> options):base(options)
        {

        }
    }
}

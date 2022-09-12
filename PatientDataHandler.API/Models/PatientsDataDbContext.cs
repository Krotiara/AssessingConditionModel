using Microsoft.EntityFrameworkCore;

namespace PatientDataHandler.API.Models
{
    public class PatientsDataDbContext: DbContext
    {
        public PatientsDataDbContext():base()
        {

        }

        public PatientsDataDbContext(DbContextOptions<PatientsDataDbContext> options):base(options)
        {

        }
    }
}

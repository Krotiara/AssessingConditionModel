using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfluenceCalculator.API.Models
{
    public class InfluenceContext: DbContext
    {
        public virtual DbSet<IInfluenceResult> InfluenceResults { get; set; }

    }
}

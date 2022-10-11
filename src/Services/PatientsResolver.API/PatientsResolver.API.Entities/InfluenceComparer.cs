using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class InfluenceComparer : IEqualityComparer<Influence>
    {
        public bool Equals(Influence? x, Influence? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;


            return x.PatientId == y.PatientId
                && x.MedicineName == y.MedicineName
                && x.StartTimestamp == y.StartTimestamp
                && x.EndTimestamp == y.EndTimestamp;
        }

        public int GetHashCode([DisallowNull] Influence obj)
        {
            return obj.PatientId.GetHashCode() 
                ^ obj.MedicineName.GetHashCode() 
                ^ obj.StartTimestamp.GetHashCode() 
                ^ obj.EndTimestamp.GetHashCode();
        }
    }
}

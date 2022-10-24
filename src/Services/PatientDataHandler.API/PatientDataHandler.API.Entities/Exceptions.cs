using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Entities
{
    public class ParseInfluenceDataException: Exception
    {
        public ParseInfluenceDataException(string message, Exception innerException):
            base(message,innerException)
        {

        }
    }
}

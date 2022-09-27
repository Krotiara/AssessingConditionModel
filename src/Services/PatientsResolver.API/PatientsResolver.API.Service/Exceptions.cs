using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Exceptions
{
    public class AddPatientException : Exception
    {
        public AddPatientException(string message):base(message)
        {

        }

        public AddPatientException(string message, Exception innerException): base(message,innerException)
        {

        }
    }
}

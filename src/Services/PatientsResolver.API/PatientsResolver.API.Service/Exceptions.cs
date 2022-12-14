using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Exceptions
{
    public class AddPatientException : Exception
    {
        public AddPatientException(string message) : 
            base(message)
        {

        }

        public AddPatientException(string message, Exception innerException) : 
            base(message, innerException)
        {

        }
    }

    public class AddPatientsRangeException : Exception
    {
        public AddPatientsRangeException(string message) :
            base(message)
        {

        }

        public AddPatientsRangeException(string message, Exception innerException) :
            base(message, innerException)
        {

        }
    }


    public class AddInfluenceException : Exception
    {
        public AddInfluenceException(string message) :
            base(message)
        {

        }

        public AddInfluenceException(string message, Exception innerException): 
            base(message, innerException)
        {

        }
    }


    public class AddInfluenceRangeException : Exception
    {
        public AddInfluenceRangeException(string message) :
            base(message)
        {

        }

        public AddInfluenceRangeException(string message, Exception innerException) :
            base(message, innerException)
        {

        }
    }


    public class GetInfluencesException : Exception
    {
        public GetInfluencesException(string message) :
            base(message)
        {

        }

        public GetInfluencesException(string message, Exception innerException) :
            base(message, innerException)
        {

        }
    }

    public class UpdatePatientException : Exception
    {
        public UpdatePatientException(string message) :
            base(message)
        {

        }

        public UpdatePatientException(string message, Exception innerException) :
            base(message, innerException)
        {

        }
    }


    public class DeletePatientException : Exception
    {
        public DeletePatientException(string message) :
            base(message)
        {

        }

        public DeletePatientException(string message, Exception innerException) :
            base(message, innerException)
        {

        }
    }
}
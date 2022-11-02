namespace TempGateway.Entities
{
    public class GetWebResponceException : Exception
    {
        public GetWebResponceException(string message) : base(message) { }

        public GetWebResponceException(string message, Exception innerException) : base(message, innerException) { }
    }


    public class AddInfluenceDataException : Exception
    {
        public AddInfluenceDataException(string message) : base(message) { }

        public AddInfluenceDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
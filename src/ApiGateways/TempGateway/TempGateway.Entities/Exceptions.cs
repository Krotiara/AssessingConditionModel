namespace TempGateway.Entities
{
    public class GetWebResponceException : Exception
    {
        public GetWebResponceException(string message) : base(message) { }

        public GetWebResponceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
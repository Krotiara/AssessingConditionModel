namespace InfluenceCalculator.API
{
    public class InfluenceCalculationException:Exception
    {
        public InfluenceCalculationException(string message, Exception innerException)
            :base(message,innerException)
        {

        }
    }
}

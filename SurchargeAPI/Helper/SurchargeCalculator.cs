namespace SurchargeAPI.Helper;

public static class SurchargeCalculator
{
    private static double _purchase;
    public static double SurchargeAmount;
    public static double Total;

    public static double CalculateSurcharge(double surchargeAmount, double total)
    {
        SurchargeAmount = surchargeAmount;
        Total = total;
        
        if (Total <= 0)
        {
            throw new ArgumentException("Total must be greater than zero.");
        }

        // Calculate Purchase if not already provided
        if (_purchase == 0)
        {
            _purchase = Total - SurchargeAmount;

            if (_purchase < 0)
            {
                throw new ArgumentException("Purchase cannot be negative. Check Total and SurchargeAmount values.");
            }
        }
        return SurchargeAmount / _purchase;
    }
}
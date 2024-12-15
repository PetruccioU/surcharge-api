namespace SurchargeAPI.Helper;

public static class SurchargeCalculator
{
    private static double Purchase;
    public static double SurchargeAmount;
    public static double Total;

    public static double CalculateSurcharge()
    {
        if (Total <= 0)
        {
            throw new ArgumentException("Total must be greater than zero.");
        }

        // Calculate Purchase if not already provided
        if (Purchase == 0)
        {
            Purchase = Total - SurchargeAmount;

            if (Purchase < 0)
            {
                throw new ArgumentException("Purchase cannot be negative. Check Total and SurchargeAmount values.");
            }
        }
        return SurchargeAmount / Purchase;
    }
}
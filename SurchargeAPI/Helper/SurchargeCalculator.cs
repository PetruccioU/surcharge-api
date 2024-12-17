namespace SurchargeAPI.Helper;

public static class SurchargeCalculator
{
    private static double _purchase;
    private static double _surchargeAmount;
    private static double _total;

    public static double CalculateSurcharge(double surchargeAmount, double total)
    {
        _purchase = 0;
        _surchargeAmount = surchargeAmount;
        _total = total;
        
        if (_total <= 0)
        {
            throw new ArgumentException("Total must be greater than zero.");
        }

        // Calculate Purchase if not already provided
        if (_purchase == 0)
        {
            _purchase = _total - _surchargeAmount;

            if (_purchase < 0)
            {
                throw new ArgumentException("Purchase cannot be negative. Check Total and SurchargeAmount values.");
            }
        }
        return _surchargeAmount / _purchase;
    }
}
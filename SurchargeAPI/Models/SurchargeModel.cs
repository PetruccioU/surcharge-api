using SurchargeAPI.Enums;
using SurchargeAPI.Helper;

namespace SurchargeAPI.Models;

public class SurchargeModel
{
    
    public SurchargeModel() { }
    
    public SurchargeModel(
        string title, 
        string description, 
        string placesApiId, 
        PaymentMethods paymentMethod, 
        double total, 
        double surcharge)
    {
        Id = Guid.NewGuid();
        PlacesApiId = placesApiId;
        // SurchargePercentage = SurchargeCalculator.CalculateSurcharge(surcharge, total);
        PaymentMethod = paymentMethod;
        Title = title;
        Description = description;
        // CreatedAt = DateTime.UtcNow;
        // UpdatedAt = DateTime.UtcNow;
    }
    
    public Guid Id { get; set; }
    public string PlacesApiId { get; set; }
    public PaymentMethods? PaymentMethod { get; set; } 
    public double SurchargePercentage => SurchargeCalculator.CalculateSurcharge(Surcharge, Total); // Computed
    public double Surcharge { get; set; }
    public double Total { get; set; }
    public string? Title { get; set; }
    public string?  Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    
}
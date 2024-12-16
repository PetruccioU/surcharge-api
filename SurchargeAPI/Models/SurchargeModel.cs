using SurchargeAPI.Enums;
using SurchargeAPI.Helper;

namespace SurchargeAPI.Models;

public class SurchargeModel
{
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
        SurchargePercentage = SurchargeCalculator.CalculateSurcharge(surcharge, total);
        PaymentMethod = paymentMethod;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public Guid Id { get; init; }
    public string PlacesApiId { get; set; }
    public PaymentMethods? PaymentMethod { get; init; } 
    public double SurchargePercentage { get; init; }
    
    public double Surcharge { get; init; }
    public double Total { get; init; }
    public string? Title { get; init; }
    public string?  Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    
}
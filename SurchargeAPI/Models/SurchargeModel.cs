using System.ComponentModel.DataAnnotations.Schema;
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
        double surcharge,
        double surchargePercentage)
    {
        Id = Guid.NewGuid();
        PlacesApiId = placesApiId;
        PaymentMethod = paymentMethod;
        Total = total;
        Surcharge = surcharge;
        SurchargePercentage = surchargePercentage;
        Title = title;
        Description = description;
    }
    
    public Guid Id { get; set; }
    public string? PlacesApiId { get; set; }
    public PaymentMethods? PaymentMethod { get; set; }
    
    public double? SurchargePercentage { get; set; }
    
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    // public double SurchargePercentage => SurchargeCalculator.CalculateSurcharge(Surcharge, Total); // Computed
    public double Surcharge { get; set; }
    public double Total { get; set; }
    public string? Title { get; set; }
    public string?  Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
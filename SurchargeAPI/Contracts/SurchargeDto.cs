using SurchargeAPI.Enums;
namespace SurchargeAPI.Contracts;

public record SurchargeDto(
    Guid Id, 
    string Title, 
    string? Description, 
    string PlacesApiId,
    PaymentMethods? PaymentMethod,
    double SurchargePercentage,
    double Surcharge,
    double Total,
    DateTime CreatedAt, 
    DateTime UpdatedAt);
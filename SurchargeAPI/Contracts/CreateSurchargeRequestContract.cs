using SurchargeAPI.Enums;

namespace SurchargeAPI.Contracts;

public record CreateSurchargeRequestContract(
    string? Title, 
    string? Description, 
    
    string PlacesApiId, 
    PaymentMethods PaymentMethod,
    
    double Total,
    double Surcharge);
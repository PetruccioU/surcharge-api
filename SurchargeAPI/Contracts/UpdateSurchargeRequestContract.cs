using SurchargeAPI.Enums;

namespace SurchargeAPI.Contracts;

public record UpdateSurchargeRequestContract(
    Guid Id, 
    string? Title, 
    string? Description,
    
    PaymentMethods? NewPaymentMethod,
    string? PlacesApiId,
    
    double? NewTotal,
    double? NewSurcharge);
namespace SurchargeAPI.Contracts;

public record SurchargeDto(
    Guid Id, 
    string Title, 
    string Description, 
    DateTime CreatedAt, 
    DateTime UpdatedAt);

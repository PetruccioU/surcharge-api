namespace SurchargeAPI.Contracts;

public record GetSurchargeRequestContract(
    string? Search, 
    string? SortItem, 
    string? SortOrder, 
    double SurchargePercentage,
    int PageNumber, 
    int PageSize);
// nullable fields
// use Capitals in the parameters names
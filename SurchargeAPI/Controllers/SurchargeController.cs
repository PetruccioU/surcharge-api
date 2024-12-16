using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurchargeAPI.Contracts;
using SurchargeAPI.DataAccess;
using SurchargeAPI.Helper;
using SurchargeAPI.Models; 

namespace SurchargeAPI.Controllers;

[ApiController]  // attribute for this controller
[Route("[controller]")] // route for this controller
public class SurchargeController : ControllerBase
{
    private readonly SurchargeDbContext _dbContext;  // create a field for storing db context

    private readonly ILogger<SurchargeController> _logger;
    
    public SurchargeController(SurchargeDbContext dbContext, ILogger<SurchargeController> logger)  // constructor method must have the same name as class
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSurchargeRequestContract request, CancellationToken ct) // put the creation method to external class "CreateNotesContract"
    {
        var surcharge = new SurchargeModel(
            request.Title, 
            request.Description, 
            request.PlacesApiId, 
            request.PaymentMethod,
            request.Total,
            request.Surcharge
            ); 
        // obtain parameters from req and store it in notes var 
        
        await _dbContext.Surcharge.AddAsync(surcharge, ct);  // add NotesModel object to db context
        await _dbContext.SaveChangesAsync(ct);  // save changes to db 
        // ToDo: learn about CancellationToken (csrf protection?) 
        
        return Ok("Post");
    }
    
    [HttpGet]  // attribute for this method 
    public async Task<IActionResult> Get([FromQuery] GetSurchargeRequestContract request, CancellationToken ct)
    {
        try
        {
            // search filtering 
            var surchargeQuery = _dbContext.Surcharge
                .Where(n => string.IsNullOrEmpty(request.Search) || 
                            n.Title.ToLower().Contains(request.Search.ToLower())
                            || n.Description.ToLower().Contains(request.Search.ToLower()));

            if (!string.IsNullOrEmpty(request.SortItem))
            {
                var selectorKey = GetSelectorKey(request.SortItem);
                // sorting 
                if (request.SortOrder == "desc")
                {
                    surchargeQuery = surchargeQuery.OrderByDescending(selectorKey);
                }
                else if (request.SortOrder == "asc")
                {
                    surchargeQuery = surchargeQuery.OrderBy(selectorKey);
                }
            } 
            
            if (!double.IsNaN(request.SurchargePercentage))
            {
                var selectorKey = GetSelectorKey(request.SortItem);
                // sorting 
                if (request.SortOrder == "desc")
                {
                    surchargeQuery = surchargeQuery.OrderByDescending(selectorKey);
                }
                else if (request.SortOrder == "asc")
                {
                    surchargeQuery = surchargeQuery.OrderBy(selectorKey);
                }
            } 
            
            
            // ToDo: Check pagination is correct
            // Pagination logic
            var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1; // Default to page 1
            var pageSize = request.PageSize > 0 ? request.PageSize : 10;     // Default to 10 items per page

            var totalRecords = await surchargeQuery.CountAsync(ct); // Get the total count of records
            var surchargeDtos = await surchargeQuery
                .Skip((pageNumber - 1) * pageSize) // Skip records for previous pages
                .Take(pageSize)                   // Take the records for the current page
                .Select(n => new SurchargeDto(
                    n.Id,
                    n.Title,
                    n.Description,
                    n.PlacesApiId,
                    n.PaymentMethod,
                    n.SurchargePercentage, 
                    n.Surcharge, 
                    n.Total, 
                    n.CreatedAt, 
                    n.UpdatedAt))
                .ToListAsync(ct);
        
            return Ok(new GetSurchargeResponseContract(surchargeDtos));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the Get request.");
            return StatusCode(500, new { message = "Internal server error", details = e.Message });
        }
        
    }

    private Expression<Func<SurchargeModel, object>> GetSelectorKey(string sortItem)
    {
        switch (sortItem)
        {
            case "createdAt": return surcharge => surcharge.CreatedAt;
            case "title": return surcharge => surcharge.Title;
            default: return surcharge => surcharge.Id;
        }
    }
    

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateSurchargeRequestContract request, CancellationToken ct)
    {
        try
        {
            var updateQuery = _dbContext.Surcharge.Where(n => n.Id == request.Id);
            
            
            double? calculatedSurcharge = null;
            // Check if NewTotal or NewSurcharge is provided to calculate the surcharge percentage
            if (request.NewTotal.HasValue && request.NewSurcharge.HasValue)
            {
                // Both Total and Surcharge are provided, calculate the percentage
                calculatedSurcharge = SurchargeCalculator.CalculateSurcharge(
                    request.NewSurcharge.Value, 
                    request.NewTotal.Value);
            }
            else if (request.NewTotal.HasValue && !request.NewSurcharge.HasValue)
            {
                var existingSurcharge = await updateQuery.Select(n => n.Surcharge).FirstOrDefaultAsync(ct);
                calculatedSurcharge = SurchargeCalculator.CalculateSurcharge(
                    existingSurcharge, 
                    request.NewTotal.Value);
            }
            else if (!request.NewTotal.HasValue && request.NewSurcharge.HasValue)
            {
                // If only NewSurcharge is provided, calculate the surcharge percentage based on the existing Total
                var existingTotal = updateQuery.Select(n => n.Total).FirstOrDefault();
                calculatedSurcharge = SurchargeCalculator.CalculateSurcharge(
                    request.NewSurcharge.Value, 
                    existingTotal);
            }

            // If a new surcharge percentage was calculated, apply it
            if (calculatedSurcharge.HasValue)
            {
                await updateQuery.ExecuteUpdateAsync(updates => updates
                    .SetProperty(n => n.SurchargePercentage, calculatedSurcharge));
            }
            
            // Perform the update
            await updateQuery.ExecuteUpdateAsync(updates => updates
                .SetProperty(n => n.Title, n => request.Title ?? n.Title)
                .SetProperty(n => n.Description, n => request.Description ?? n.Description)
                .SetProperty(n => n.PaymentMethod, request.NewPaymentMethod)
                .SetProperty(n => n.PlacesApiId, request.PlacesApiId)
                .SetProperty(n => n.UpdatedAt, _ => DateTime.UtcNow), ct);
            
            await _dbContext.SaveChangesAsync(ct);
            return Ok(new { message = "Note updated successfully" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the Update request.");
            return StatusCode(500, new { message = "Internal server error", details = e.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteSurchargeRequestContract request, CancellationToken ct)
    {
        try
        {
            await _dbContext.Surcharge
                .Where(n => n.Id.Equals(request.Id))
                .ExecuteDeleteAsync(ct);  // remove from db context
            await _dbContext.SaveChangesAsync(ct);
            return Ok("Delete");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the Delete request.");
            return StatusCode(500, new { message = "Internal server error", details = e.Message });
        }
        
    }
}
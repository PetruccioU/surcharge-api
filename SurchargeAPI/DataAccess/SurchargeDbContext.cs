using Microsoft.EntityFrameworkCore;
using SurchargeAPI.Models;

namespace SurchargeAPI.DataAccess;

public class SurchargeDbContext: DbContext
{
    private readonly IConfiguration _configuration;

    public SurchargeDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<SurchargeModel> Surcharge => Set<SurchargeModel>(); // Todo: learn about DbSet

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));
    }
    
}
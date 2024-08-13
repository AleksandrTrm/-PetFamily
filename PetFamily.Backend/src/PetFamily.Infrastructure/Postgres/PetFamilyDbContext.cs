using Microsoft.EntityFrameworkCore;

namespace PetFamily.Infrastructure.Postgres
{
    public class PetFamilyDbContext : DbContext
    {
        
        public PetFamilyDbContext(DbContextOptions<PetFamilyDbContext> options) : base(options) { }
    }
    
}
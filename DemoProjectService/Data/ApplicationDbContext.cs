using DemoProjectService.Entity;
using Microsoft.EntityFrameworkCore;
using DemoProjectService.Entity;
namespace DemoProjectService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}

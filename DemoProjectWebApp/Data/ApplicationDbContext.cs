using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DemoProjectWebApp.Models;


namespace Mvc.RoleAuthorization.Data
{
    public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		
        protected override void OnModelCreating(ModelBuilder builder)
		{
		}
	}
}
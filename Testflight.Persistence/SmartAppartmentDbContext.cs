using Microsoft.EntityFrameworkCore;
using Testflight.Domain.Entities;
using Testflight.Persistence.Interfaces;

namespace Testflight.Persistence
{
	public class SmartAppartmentDbContext : DbContext, ISmartAppartmentDbContext
	{
		public SmartAppartmentDbContext(DbContextOptions<SmartAppartmentDbContext> options) : base(options) { }

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartAppartmentDbContext).Assembly);
		}
	}
}

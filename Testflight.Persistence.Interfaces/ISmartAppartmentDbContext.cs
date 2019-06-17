using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Testflight.Domain.Entities;

namespace Testflight.Persistence.Interfaces
{
	public interface ISmartAppartmentDbContext
	{
		DbSet<Customer> Customers { get; set; }
		DbSet<Product> Products { get; set; }
		DbSet<Order> Orders { get; set; }
		DbSet<OrderDetail> OrderDetails { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}

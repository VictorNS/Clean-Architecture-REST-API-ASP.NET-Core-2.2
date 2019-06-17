using System;
using Testflight.Domain.Entities;

namespace Testflight.Persistence.InMemory
{
	public class SmartAppartmentDbInitializer
	{
		public static void Initialize(SmartAppartmentDbContext context)
		{
			var initializer = new SmartAppartmentDbInitializer();

			initializer.SeedCustomers(context);
			initializer.SeedProducts(context);
			initializer.SeedOrders(context);
			initializer.SeedOrderDetails(context);
		}
		private void SeedCustomers(SmartAppartmentDbContext context)
		{
			context.Customers.AddRange(new Customer[]
			{
				new Customer { CustomerId = 1, CompanyName = "Company 1", Address = "Street 1"},
				new Customer { CustomerId = 2, CompanyName = "Company 2", Address = "Street 2"},
				new Customer { CustomerId = 3, CompanyName = "Company 3", Address = "Street 3"},
			});
			context.SaveChanges();
		}
		private void SeedProducts(SmartAppartmentDbContext context)
		{
			context.Products.AddRange(new Product[]
			{
				new Product { ProductId = 1, ProductName = "Product 1", UnitPrice = 11 },
				new Product { ProductId = 2, ProductName = "Product 2", UnitPrice = 22 },
				new Product { ProductId = 3, ProductName = "Product 2", UnitPrice = 33 },
			});
			context.SaveChanges();
		}
		private void SeedOrders(SmartAppartmentDbContext context)
		{
			context.Orders.AddRange(new Order[]
			{
				new Order { OrderId = 1, CustomerId = 1, OrderDate = new DateTime(2019, 01, 01), ShippedDate = null },
				new Order { OrderId = 2, CustomerId = 2, OrderDate = new DateTime(2019, 02, 02), ShippedDate = new DateTime(2019, 04, 04) },
				new Order { OrderId = 3, CustomerId = 3, OrderDate = new DateTime(2019, 03, 03), ShippedDate = null },
			});
			context.SaveChanges();
		}
		private void SeedOrderDetails(SmartAppartmentDbContext context)
		{
			context.OrderDetails.AddRange(new OrderDetail[]
			{
				new OrderDetail { OrderDetailId = 11, OrderId = 1, ProductId = 1, Quantity = 100, UnitPrice = 11 },
				new OrderDetail { OrderDetailId = 12, OrderId = 1, ProductId = 2, Quantity = 200, UnitPrice = 22 },

				new OrderDetail { OrderDetailId = 21, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 1000 },

				new OrderDetail { OrderDetailId = 31, OrderId = 3, ProductId = 2, Quantity = 1, UnitPrice = 1000 },
			});
			context.SaveChanges();
		}
	}
}

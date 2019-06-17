using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testflight.Domain.Entities;

namespace Testflight.Persistence.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.HasKey(e => e.OrderId);
			builder.Property(e => e.OrderId).HasColumnName("OrderID");

			builder.Property(e => e.CustomerId).HasColumnName("CustomerID");
			builder.Property(e => e.OrderDate).HasColumnType("datetime");
			builder.Property(e => e.ShippedDate).HasColumnType("datetime");

			builder.HasOne(d => d.Customer)
				.WithMany(p => p.Orders)
				.HasForeignKey(d => d.CustomerId)
				.HasConstraintName("FK_Orders_Customers");
		}
	}
}

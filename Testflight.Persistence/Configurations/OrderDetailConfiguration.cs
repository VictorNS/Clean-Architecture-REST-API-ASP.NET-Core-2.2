using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testflight.Domain.Entities;

namespace Testflight.Persistence.Configurations
{
	public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
	{
		public void Configure(EntityTypeBuilder<OrderDetail> builder)
		{
			builder.HasKey(e => new { e.OrderDetailId });
			builder.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

			builder.Property(e => e.OrderId).HasColumnName("OrderID");
			builder.Property(e => e.ProductId).HasColumnName("ProductID");
			builder.Property(e => e.UnitPrice).HasColumnType("money");
			builder.Property(e => e.Quantity).HasDefaultValueSql("((1))");

			builder.HasOne(d => d.Order)
				.WithMany(p => p.OrderDetails)
				.HasForeignKey(d => d.OrderId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Order_Details_Orders");

			builder.HasOne(d => d.Product)
				.WithMany(p => p.OrderDetails)
				.HasForeignKey(d => d.ProductId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Order_Details_Products");
		}
	}
}

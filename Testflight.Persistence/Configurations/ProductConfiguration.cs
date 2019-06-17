using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testflight.Domain.Entities;

namespace Testflight.Persistence.Configurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.HasKey(e => e.ProductId);
			builder.Property(e => e.ProductId).HasColumnName("ProductID");

			builder.Property(e => e.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(e => e.UnitPrice)
				.HasColumnType("money")
				.HasDefaultValueSql("((0))");
		}
	}
}

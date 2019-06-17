using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testflight.Domain.Entities;

namespace Testflight.Persistence.Configurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.HasKey(e => e.CustomerId);
			builder.Property(e => e.CustomerId)
				.HasColumnName("CustomerID")
				.ValueGeneratedNever();

			builder.Property(e => e.CompanyName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(e => e.Address).HasMaxLength(60);
		}
	}
}

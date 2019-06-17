using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Testflight.Application.Exceptions;
using Testflight.Application.Products;
using Testflight.Persistence;
using Xunit;

namespace Testflight.Application.Tests.Products
{
	public class ProductService_GetAsync
	{
		ILogger<ProductService> _logger;

		public ProductService_GetAsync()
		{
			var mock = new Mock<ILogger<ProductService>>();
			_logger = mock.Object;
		}

		private DbContextOptions<SmartAppartmentDbContext> CreateContextOptions([CallerMemberName]string databaseName = "")
		{
			var options = new DbContextOptionsBuilder<SmartAppartmentDbContext>()
				 .UseInMemoryDatabase(databaseName)
				 .Options;
			return options;
		}

		[Fact]
		public async void ShoudReturnOneDto()
		{
			var options = CreateContextOptions();
			using (var context = new SmartAppartmentDbContext(options))
			{
				Persistence.InMemory.SmartAppartmentDbInitializer.Initialize(context);
			}

			var expected = new ProductDto { ProductId = 1, ProductName = "Product 1", UnitPrice = 11 };

			using (var context = new SmartAppartmentDbContext(options))
			{
				var service = new ProductService(context, _logger);
				var actual = await service.GetAsync(1, CancellationToken.None);
				actual.Should().BeEquivalentTo(expected);
			}
		}

		[Fact]
		public async void ShoudThrowException()
		{
			var options = CreateContextOptions();
			using (var context = new SmartAppartmentDbContext(options))
			{
				Persistence.InMemory.SmartAppartmentDbInitializer.Initialize(context);
			}

			using (var context = new SmartAppartmentDbContext(options))
			{
				var service = new ProductService(context, _logger);
				Func<Task> act = async () => await service.GetAsync(4, CancellationToken.None);
				await act.Should()
					.ThrowAsync<NotFoundException>()
					.WithMessage("Entity \"Product\" (4) was not found.");
			}
		}
	}
}

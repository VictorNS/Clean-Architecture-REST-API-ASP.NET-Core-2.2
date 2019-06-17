using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Testflight.Application.Exceptions;
using Testflight.Domain.Entities;
using Testflight.Persistence.Interfaces;

namespace Testflight.Application.Products
{
	public class ProductService : IProductService
	{
		private readonly ISmartAppartmentDbContext _context;
		private readonly ILogger<ProductService> _logger;

		public ProductService(ISmartAppartmentDbContext context, ILogger<ProductService> logger)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IList<ProductSimpleDto>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await _context.Products.Select(x => new ProductSimpleDto
			{
				ProductId = x.ProductId,
				ProductName = x.ProductName
			}).ToListAsync(cancellationToken);
		}
		public async Task<ProductDto> GetAsync(int id, CancellationToken cancellationToken)
		{
			var entity = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
			if (entity == null)
			{
				_logger.LogWarning("Bad request ... add some data ... maybe PK");
				throw new NotFoundException(nameof(Product), id);
			}
			return new ProductDto
			{
				ProductId = entity.ProductId,
				ProductName = entity.ProductName,
				UnitPrice = entity.UnitPrice
			};
		}
		public async Task<int> AddAsync(ProductDto dto, CancellationToken cancellationToken)
		{
			// TODO validation
			var entity = new Product
			{
				ProductId = dto.ProductId,
				ProductName = dto.ProductName,
				UnitPrice = dto.UnitPrice
			};
			_context.Products.Add(entity);
			await _context.SaveChangesAsync(cancellationToken);

			return entity.ProductId;
		}
		public async Task UpdateAsync(ProductDto dto, CancellationToken cancellationToken)
		{
			// TODO validation
			var entity = await _context.Products.FindAsync(dto.ProductId);

			if (entity == null)
			{
				throw new NotFoundException(nameof(Product), dto.ProductId);
			}

			try
			{
				entity.ProductId = dto.ProductId;
				entity.ProductName = dto.ProductName;
				entity.UnitPrice = dto.UnitPrice;

				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new ConflictException(nameof(Product), dto.ProductId);
			}
		}
		public async Task RemoveAsync(int id, CancellationToken cancellationToken)
		{
			var entity = await _context.Products.FindAsync(id);

			if (entity == null)
			{
				throw new NotFoundException(nameof(Product), id);
			}

			_context.Products.Remove(entity);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}

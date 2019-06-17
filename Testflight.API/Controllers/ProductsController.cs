using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Testflight.Application.Products;

namespace Testflight.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService ?? throw new ArgumentNullException(nameof(productService));
		}

		[HttpGet]
		public async Task<ActionResult<IList<ProductSimpleDto>>> GetAll(CancellationToken cancellationToken)
		{
			return Ok(await _productService.GetAllAsync(cancellationToken));
		}

		[HttpGet("{id}")]
		[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
		public async Task<ActionResult<ProductDto>> Get(int id, CancellationToken cancellationToken)
		{
			return Ok(await _productService.GetAsync(id, cancellationToken));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> Create([FromBody]ProductDto dto, CancellationToken cancellationToken)
		{
			await _productService.AddAsync(dto, cancellationToken);

			return NoContent();
		}

		[HttpPut()]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> Update([FromBody]ProductDto dto, CancellationToken cancellationToken)
		{
			await _productService.UpdateAsync(dto, cancellationToken);

			return NoContent();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
		{
			await _productService.RemoveAsync(id, cancellationToken);

			return NoContent();
		}
	}
}

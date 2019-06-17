using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Testflight.Application.Products
{
	public interface IProductService
	{
		Task<IList<ProductSimpleDto>> GetAllAsync(CancellationToken cancellationToken);
		Task<ProductDto> GetAsync(int id, CancellationToken cancellationToken);
		Task<int> AddAsync(ProductDto dto, CancellationToken cancellationToken);
		Task UpdateAsync(ProductDto dto, CancellationToken cancellationToken);
		Task RemoveAsync(int id, CancellationToken cancellationToken);
	}
}

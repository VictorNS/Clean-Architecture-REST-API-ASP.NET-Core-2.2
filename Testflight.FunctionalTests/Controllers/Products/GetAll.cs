using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Testflight.FunctionalTests.Controllers.Products
{
	public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
	{
		private readonly HttpClient _client;

		public GetAll(CustomWebApplicationFactory<Startup> factory)
		{
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task ReturnsProducts()
		{
			var response = await _client.GetAsync("/products");

			response.EnsureSuccessStatusCode();

			// TODO parse and check the response
		}
	}
}

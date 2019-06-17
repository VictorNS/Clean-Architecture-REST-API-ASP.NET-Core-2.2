using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Testflight.Infrustructure
{
	public class SimpleHealthCheck : IHealthCheck
	{
		private bool healthCheckResultHealthy;
		public SimpleHealthCheck()
		{
			// TODO check dbContext status
			healthCheckResultHealthy = true;
		}

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (healthCheckResultHealthy)
			{
				return Task.FromResult(HealthCheckResult.Healthy());
			}
			return Task.FromResult(HealthCheckResult.Unhealthy());
		}
	}
}

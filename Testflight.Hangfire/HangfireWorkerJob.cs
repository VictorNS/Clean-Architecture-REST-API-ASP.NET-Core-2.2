using System;
using System.Threading.Tasks;
using Hangfire;
using Testflight.Application.HangfireTest;

namespace Testflight.Hangfire
{
	public class HangfireWorkerJob
	{
		/// <summary>
		/// It's an example of using DI, we are not going to keep a logic in a job.
		/// </summary>
		private IHangfireWorkerService _realWorker;

		public HangfireWorkerJob(IHangfireWorkerService realWorker)
		{
			_realWorker = realWorker ?? throw new ArgumentNullException(nameof(realWorker));
		}

		[AutomaticRetry(Attempts = 3)]
		public async Task RunAsync(string input)
		{
			// It's an example of sending a parameter to a service.
			await _realWorker.RunAsync(input);
		}
	}
}

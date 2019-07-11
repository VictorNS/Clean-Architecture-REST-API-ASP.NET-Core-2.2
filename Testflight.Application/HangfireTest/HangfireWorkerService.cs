using System;
using System.Threading.Tasks;

namespace Testflight.Application.HangfireTest
{
	public interface IHangfireWorkerService
	{
		Task RunAsync(string text);
	}

	public class HangfireWorkerService : IHangfireWorkerService
	{
		private readonly string _workerId;

		public HangfireWorkerService()
		{
			this._workerId = Guid.NewGuid().ToString();
		}

		public async Task RunAsync(string text)
		{
			await Task.Run(() =>
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Text: {text}, WorkerId: {_workerId}, At: {DateTime.Now}");
				Console.ForegroundColor = ConsoleColor.White;
				// If you need repeat a task - just throw an exception.
				throw new Exception("=== Repeat the task, please ===");
			});
		}
	}
}

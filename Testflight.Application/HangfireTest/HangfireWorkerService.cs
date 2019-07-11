using System;

namespace Testflight.Application.HangfireTest
{
	public interface IHangfireWorkerService
	{
		void Run(string text);
	}

	public class HangfireWorkerService : IHangfireWorkerService
	{
		private readonly string _workerId;

		public HangfireWorkerService()
		{
			this._workerId = Guid.NewGuid().ToString();
		}

		public void Run(string text)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Text: {text}, WorkerId: {_workerId}, At: {DateTime.Now}");
			Console.ForegroundColor = ConsoleColor.White;
			// If you need repeat a task - just throw an exception.
			throw new Exception("=== Repeat the task, please ===");
		}
	}
}

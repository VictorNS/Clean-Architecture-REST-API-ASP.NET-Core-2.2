using System;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Testflight.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[ApiConventionType(typeof(DefaultApiConventions))]
	public class HangfireTestController : ControllerBase
	{
		IBackgroundJobClient _backgroundJobs;

		public HangfireTestController(IBackgroundJobClient backgroundJobs)
		{
			_backgroundJobs = backgroundJobs ?? throw new ArgumentNullException(nameof(backgroundJobs));
		}

		public IActionResult Index()
        {
			var jobId = _backgroundJobs.Enqueue<Hangfire.HangfireWorkerJob>(j => j.RunAsync("Call from HangfireTestController"));
			return Ok(new { jobId });
        }
    }
}

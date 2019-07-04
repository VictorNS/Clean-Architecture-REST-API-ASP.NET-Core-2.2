using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Testflight.Infrustructure.Authorization;

namespace Testflight.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[ApiConventionType(typeof(DefaultApiConventions))]
	public class ValuesController : ControllerBase
	{
		[HttpGet]
		[Authorize]
		public ActionResult<IEnumerable<string>> Get()
		{
			return new string[] { "value 1", "value 2" };
		}

		[HttpGet("{id}")]
		[Authorize("read:data")]
		public ActionResult<string> Get(int id)
		{
			return $"value {id}";
		}

		[HttpGet("report")]
		[Authorize("read:reports")]
		public ActionResult<string> GetReport()
		{
			return "There are two items in a repository.";
		}

		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		[HttpDelete("{id}")]
		[Authorize("custom_header")]
		public void Delete([FromServices] CustomHeaderModel customHeader, int id)
		{
			if (customHeader.SecretId != id)
				throw new Exception($"Invalid header with secretId: {customHeader.SecretId}");
		}
	}
}

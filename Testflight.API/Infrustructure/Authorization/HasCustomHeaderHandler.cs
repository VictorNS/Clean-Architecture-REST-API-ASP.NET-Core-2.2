using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Testflight.Infrustructure.Authorization
{
	public class HasCustomHeaderHandler : AuthorizationHandler<HasCustomHeaderRequirement>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HasCustomHeaderHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasCustomHeaderRequirement requirement)
		{
			if (HasSecretId(_httpContextAccessor, out _))
				context.Succeed(requirement);

			return Task.CompletedTask;
		}

		public static bool HasSecretId(IHttpContextAccessor _httpContextAccessor, out int id)
		{
			var authHeader = _httpContextAccessor.HttpContext.Request.Headers["AuthorizationCustom"];

			string[] headerParts = authHeader.ToString().Split(':');
			if (headerParts.Length != 2 || headerParts[0] != "secretid")
			{
				id = -1;
				return false;
			}

			return int.TryParse(headerParts[1], out id);
		}
	}
}

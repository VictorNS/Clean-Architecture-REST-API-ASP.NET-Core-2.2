using System;
using Microsoft.AspNetCore.Authorization;

namespace Testflight.Infrustructure.Authorization
{
	public class HasCustomHeaderRequirement : IAuthorizationRequirement
	{
		public HasCustomHeaderRequirement() { }
	}
}

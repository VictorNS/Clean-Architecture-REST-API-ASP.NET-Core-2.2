using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Testflight.Application.Exceptions;

namespace Testflight.Infrustructure
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			ProblemDetails p;

			if (context.Exception is NotFoundException)
			{
				p = new ProblemDetails
				{
					Status = (int)HttpStatusCode.NotFound,
					Title = context.Exception.Message
				};
			}
			else if (context.Exception is ConflictException)
			{
				p = new ProblemDetails
				{
					Status = (int)HttpStatusCode.Conflict,
					Title = context.Exception.Message
				};
			}
			else
			{
				p = new ProblemDetails
				{
					Status = (int)HttpStatusCode.InternalServerError,
					Title = context.Exception.Message
				};
			}
#if DEBUG
			p.Detail = context.Exception.ToString();
#endif

			context.HttpContext.Response.ContentType = "application/json";
			context.HttpContext.Response.StatusCode = p.Status.Value;
			context.Result = new JsonResult(p);
		}
	}
}

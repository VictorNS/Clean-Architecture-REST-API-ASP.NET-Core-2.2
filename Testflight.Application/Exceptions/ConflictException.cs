using System;

namespace Testflight.Application.Exceptions
{
	public class ConflictException : Exception
	{
		public ConflictException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.") { }
	}
}

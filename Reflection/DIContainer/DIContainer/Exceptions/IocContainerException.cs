using System;

namespace DIContainer.Exceptions
{
	public class IoCContainerException : Exception
	{
		public IoCContainerException(string message) : base(message)
		{
		}
	}
}

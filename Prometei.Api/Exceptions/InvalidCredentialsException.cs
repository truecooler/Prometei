using System;
using System.Collections.Generic;
using System.Text;

namespace Prometei.Api.Exceptions
{
	[Serializable]
	class InvalidCredentialsException : Exception
	{
		public InvalidCredentialsException(string message)
			: base(message)
		{

		}

		public InvalidCredentialsException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}

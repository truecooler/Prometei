using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Prometei.Api.Exceptions
{
	[Serializable]
	class ServerErrorException : Exception
	{
		public HttpStatusCode StatusCode { get; set; }
		public HttpContent Content { get; set; }

		public ServerErrorException(string message, HttpStatusCode statusCode, HttpContent Content, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}

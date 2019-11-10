using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Prometei.Api.Models
{
	public class User
	{
		public string Login { get; private set; }

		public string Name { get; private set; }

		public string Email { get; private set; }

		public static User FromHttpContent(HttpContent httpContent)
		{
			var htmlDocument = Utils.ByteArrayTo1251String(httpContent.ReadAsByteArrayAsync().Result);
			var match = Constants.ViewProfilePage.RegularExpressions.ProfileProperties.Match(htmlDocument);
			var login = Regex.Match(htmlDocument,"<form method=\"post\" action=\"mail\\.asp\\?action=send&to=(.+?)&").Groups[1].Value;
			var name = Regex.Match(htmlDocument, "<div style=\"color: black; padding-top: 2px;\"><div>(.+?)</div></div>").Groups[1].Value;
			var email = Regex.Match(htmlDocument, "<input type=\"hidden\" name=\"mails\" value=\"(.+?)\" />").Groups[1].Value;

			return new User() { Login = login, Name = name, Email = email };
		}

	}
}

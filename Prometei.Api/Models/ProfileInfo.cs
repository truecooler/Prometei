using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace Prometei.Api.Models
{
	public class ProfileInfo
	{
		public string Login { get; private set; }

		public string Password { get; private set; }

		public string Name { get; private set; }

		public string Surname { get; private set; }

		public string Patronymic { get; private set; }

		public string Departament { get; private set; }

		public string Position { get; private set; }

		public string Phone { get; private set; }

		public Uri PhotoUri { get; private set; }

		public static ProfileInfo FromHttpContent(HttpContent httpContent)
		{
			var htmlDocument = Utils.ByteArrayTo1251String(httpContent.ReadAsByteArrayAsync().Result);
			var matches = Constants.ViewProfilePage.RegularExpressions.ProfileProperties.Matches(htmlDocument);
			var properties = new Dictionary<string, string>();
			
			foreach(var match in matches.OfType<Match>())
			{
				properties.Add(match.Groups[1].Value, match.Groups[2].Value);
			}

			var result = new ProfileInfo();
			result.Login = properties[Constants.ViewProfilePage.LoginLabel];
			result.Password = properties[Constants.ViewProfilePage.PasswordLabel];
			result.Surname = properties[Constants.ViewProfilePage.SurnameLabel];
			result.Name = properties[Constants.ViewProfilePage.NameLabel];
			result.Phone = properties[Constants.ViewProfilePage.PhoneLabel];
			result.Patronymic = properties[Constants.ViewProfilePage.PatronymicLabel];
			result.Departament = properties[Constants.ViewProfilePage.DepartamentLabel];
			result.Position = properties[Constants.ViewProfilePage.PositionLabel];

			var urlRelativeUrl = Constants.ViewProfilePage.RegularExpressions.ProfilePhotoUrl.Match(htmlDocument).Groups[1].Value;

			result.PhotoUri = new Uri(Constants.PrometeiUri, urlRelativeUrl);

			return result;
		}
	}
}

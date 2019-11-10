using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Prometei.Api
{
	public static class Constants
	{
		public static readonly Uri PrometeiUri = new Uri("http://dot.mpei.ac.ru:8081/");

		public const string EscapedHtmlSpace = "&nbsp;";
		public static class DefaultPage
		{
			public static readonly Uri Uri = new Uri(PrometeiUri, "close/default.asp");
		}

		public static class CourseEventsPage
		{
			public static readonly Uri Uri = new Uri(PrometeiUri, "close/students/ktpEvents.asp");
			public static class RequestParameters
			{
				public const string CourseEventId = "idKtp";
			}
			public static class RegularExpressions
			{
				public static readonly Regex EventsList = new Regex("<tr.*?><td>(.+?)</td><td>(.+?)</td><td>(.+?) - (.+?)</td><td>(.*?)&nbsp;</td><td>(.*?)</td><td.*?>(.*?)</td></tr>");
			}
		}
		public static class DashboardPage
		{
			public static readonly Uri Uri = new Uri(PrometeiUri, "close/students/info.asp");

			public static class RegularExpressions
			{
				public static readonly Regex ExamsList = new Regex("<tr class=\"exam\" course-id=\"(.+?)\" test-id=\"(.+?)\">.+?<td>(.+?)</td>.+?<td class=\"test-name\".+?>(.+?)</td>.+?<td class=\"course-name\".+?>(.+?)</td>.+?<td class=\"access-cnt a\">(.+?)</td>.+?</tr>", RegexOptions.Singleline);
				public static readonly Regex CoursesList = new Regex("<td><a href=\"ktpEvents\\.asp\\?idKtp=(.+?)\".+?</a></td>.+?<a href=\"books/course\\.asp\\?id=(.+?)\">(.+?)</a>", RegexOptions.Singleline);
			}
		}
		public static class ViewProfilePage
		{
			public static readonly Uri Uri = new Uri(PrometeiUri, "close/students/viewProfile.asp");

			public const string LoginLabel = "Логин";
			public const string PasswordLabel = "Пароль";
			public const string SurnameLabel = "Фамилия";
			public const string NameLabel = "Имя";
			public const string PatronymicLabel = "Отчество";
			public const string PhoneLabel = "Телефон";
			public const string DepartamentLabel = "Подразделение";
			public const string PositionLabel = "Должность";
			public static class RegularExpressions
			{
				public static readonly Regex ProfileProperties = new Regex("<div class='col-xs-3'.+?>(.+?)</div><div class='col-xs-3'>(.+?)</div>");
				public static readonly Regex ProfilePhotoUrl = new Regex("<img src='(.+)'.+id=\"Photo\">");
			}
		}
	}
}


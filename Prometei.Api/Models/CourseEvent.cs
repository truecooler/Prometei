using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Prometei.Api.Models
{
	public class CourseEvent
	{
		public class Types
		{
			public const string Lecture = "лекция";
			public const string Test = "тест";
			public const string Meeting = "орг. мероприятие";
			public const string FileExchange = "орг. мероприятие";
			public const string Other = "другое";
		}
		public string Name { get; private set; }

		public string Type { get; private set; }

		public DateTime TimeBegin { get; private set; }

		public DateTime TimeEnd { get; private set; }

		public int? ResultScore { get; private set; }

		public static IEnumerable<CourseEvent> ListFromHttpContent(HttpContent httpContent)
		{
			var htmlDocument = Utils.ByteArrayTo1251String(httpContent.ReadAsByteArrayAsync().Result);
			var matches = Constants.CourseEventsPage.RegularExpressions.EventsList.Matches(htmlDocument);

			foreach (var match in matches.OfType<Match>())
			{
				var courseEvent = new CourseEvent();
				courseEvent.Name = match.Groups[1].Value;
				courseEvent.Type = match.Groups[2].Value;
				courseEvent.TimeBegin = DateTime.Parse(match.Groups[3].Value);
				courseEvent.TimeEnd = DateTime.Parse(match.Groups[4].Value);
				courseEvent.ResultScore = (int.TryParse(match.Groups[5].Value, out int score) == true) ? score : default(int?);
				yield return courseEvent;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Prometei.Api.Models
{
	public class Course
	{
		public Guid EventsId { get; private set; }

		public Guid Id { get; private set; }

		public string Name { get; private set; }

		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ Id.ToString().GetHashCode() ^ EventsId.ToString().GetHashCode();
		}

		public static IEnumerable<Course> ListFromHttpContent(HttpContent httpContent)
		{
			var htmlDocument = Utils.ByteArrayTo1251String(httpContent.ReadAsByteArrayAsync().Result);
			var matches = Constants.DashboardPage.RegularExpressions.CoursesList.Matches(htmlDocument);

			foreach (var match in matches.OfType<Match>())
			{
				var course = new Course();
				course.EventsId = Guid.Parse(match.Groups[1].Value);
				course.Id = Guid.Parse(match.Groups[2].Value);
				course.Name = match.Groups[3].Value;
				yield return course;
			}
		}
	}
}
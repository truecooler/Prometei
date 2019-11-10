using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prometei.Api.Models
{
	public class Exam
	{
		public DateTime? TimeBegin { get; private set; }
		public DateTime TimeEnd { get; private set; }
		public Guid CourseId { get; private set; }
		public Guid TestId { get; private set; }
		public string TestName { get; private set; }
		public string CourseName { get; private set; }
		public int TriesCount { get; private set; }

		public static IEnumerable<Exam> ListFromHttpContent(HttpContent httpContent)
		{
			var htmlDocument = Utils.ByteArrayTo1251String(httpContent.ReadAsByteArrayAsync().Result);
			var matches = Constants.DashboardPage.RegularExpressions.ExamsList.Matches(htmlDocument);

			foreach (var match in matches.OfType<Match>())
			{
				var exam = new Exam();
				exam.CourseId = Guid.Parse(match.Groups[1].Value);
				exam.TestId = Guid.Parse(match.Groups[2].Value);
				exam.FillTimeSpan(match.Groups[3].Value);
				exam.TestName = match.Groups[4].Value;
				exam.CourseName = match.Groups[5].Value;
				exam.TriesCount = int.Parse(match.Groups[6].Value);
				yield return exam;
			}
		}

		private void FillTimeSpan(string data)
		{
			var dateTimes = data.Split(new string[] { Constants.EscapedHtmlSpace }, StringSplitOptions.RemoveEmptyEntries).ToList();
			dateTimes.RemoveAll(x => x.Length < 3);

			if (dateTimes.Count == 1)
			{
				this.TimeEnd = DateTime.Parse(dateTimes.Single());
			}
			else if (dateTimes.Count == 2)
			{
				this.TimeBegin = DateTime.Parse(dateTimes.First());
				this.TimeEnd = DateTime.Parse(dateTimes.Last());
			}
			else
			{
				throw new InvalidOperationException("Error occured while parsing timespans for exams");
			}
		}
	}
}

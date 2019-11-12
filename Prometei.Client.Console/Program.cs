using Prometei.Api;
using System.Linq;
using System;
using Prometei.Api.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Prometei.Client.Console
{
	class Program
	{
		static async Task Main(string[] args)
		{
			System.Console.WriteLine("your login:");
			string login = System.Console.ReadLine();

			System.Console.WriteLine("your pass:");
			string password = System.Console.ReadLine();

			var api = new PrometeiApi(login, password);
			await api.EnsureCredentialsAreValidAsync();

			var activeCourses = new Dictionary<Course, List<CourseEvent>>();

			var myCourses = (await api.GetCouresAsync()).ToList();

			foreach (var cource in myCourses)
			{
				var courceEvents = (await api.GetCoureEventsAsync(cource.EventsId)).ToList();
				var activeCourceEvents = courceEvents.Where(x => x.TimeBegin < DateTime.Now && x.TimeEnd > DateTime.Now && x.Type != CourseEvent.Types.Lecture).ToList();
				if (activeCourceEvents.Any())
				{
					activeCourses.Add(cource, activeCourceEvents);
				}
			}

			System.Console.WriteLine($"total active courses: {activeCourses.Count} with {activeCourses.Sum(x=>x.Value.Count)} events:");
			System.Console.WriteLine();
			foreach (var activeCourse in activeCourses)
			{
				System.Console.WriteLine($"course '{activeCourse.Key.Name}' has {activeCourse.Value.Count} active events:");
				foreach (var activeCourseEvent in activeCourse.Value)
				{
					System.Console.WriteLine($" {activeCourseEvent.Name} -> {activeCourseEvent.Type} -> {activeCourseEvent.TimeBegin} - {activeCourseEvent.TimeEnd}");
				}
				System.Console.WriteLine();
			}
			System.Console.ReadKey();
		}
	}
}

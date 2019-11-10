using Prometei.Api;
using System.Linq;
using System;
using Prometei.Api.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prometei.Client.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Count() != 2)
			{
				System.Console.WriteLine($"usage: ./{Process.GetCurrentProcess().ProcessName} login password");
				return;
			}

			var api = new PrometeiApi(args[0], args[1]);
			api.EnsureCredentialsAreValidAsync().Wait();

			var activeCourses = new Dictionary<Course, List<CourseEvent>>();

			var myCourses = api.GetCouresAsync().Result.ToList();

			foreach (var cource in myCourses)
			{
				var courceEvents = api.GetCoureEventsAsync(cource.EventsId).Result.ToList();
				var activeCourceEvents = courceEvents.Where(x => x.TimeBegin < DateTime.Now && x.TimeEnd > DateTime.Now).ToList();
				if (activeCourceEvents.Any())
				{
					activeCourses.Add(cource, activeCourceEvents);
				}
			}

			System.Console.WriteLine($"total active courses: {activeCourses.Count} with {activeCourses.Sum(x=>x.Value.Count)} events");
			foreach (var activeCourse in activeCourses)
			{
				System.Console.WriteLine($"course '{activeCourse.Key.Name}' has {activeCourse.Value.Count} active events:");
				foreach (var activeCourseEvent in activeCourse.Value)
				{
					System.Console.WriteLine($"\t{activeCourseEvent.Name} -> {activeCourseEvent.Type} -> {activeCourseEvent.TimeBegin} - {activeCourseEvent.TimeEnd}");
				}
			}
			System.Console.ReadKey();
		}
	}
}

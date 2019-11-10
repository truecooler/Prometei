using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Prometei.Api.Exceptions;
using Prometei.Api.Models;

namespace Prometei.Api
{
	public class PrometeiApi
	{
		private HttpClient httpClient = new HttpClient(new HttpClientHandler() { MaxConnectionsPerServer = 30 });

		public PrometeiApi(string login, string password)
		{
			var credentials = Encoding.ASCII.GetBytes($"{login}:{password}");
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
			httpClient.DefaultRequestHeaders.ConnectionClose = true;
		}

		public async Task EnsureCredentialsAreValidAsync()
		{
			HttpResponseMessage response = await httpClient.GetAsync(Constants.DefaultPage.Uri);
			EnsureResponseCompletedSuccessfully(response);
		}

		public async Task<ProfileInfo> GetProfileInfoAsync()
		{
			HttpResponseMessage response = await httpClient.GetAsync(Constants.ViewProfilePage.Uri);
			EnsureResponseCompletedSuccessfully(response);

			return ProfileInfo.FromHttpContent(response.Content);
		}

		public async Task<IEnumerable<Exam>> GetExamsAsync()
		{
			HttpResponseMessage response = await httpClient.GetAsync(Constants.DashboardPage.Uri);
			EnsureResponseCompletedSuccessfully(response);

			return Exam.ListFromHttpContent(response.Content);
		}

		public async Task<IEnumerable<Course>> GetCouresAsync()
		{
			HttpResponseMessage response = await httpClient.GetAsync(Constants.DashboardPage.Uri);
			EnsureResponseCompletedSuccessfully(response);

			return Course.ListFromHttpContent(response.Content);
		}

		public async Task<IEnumerable<CourseEvent>> GetCoureEventsAsync(Guid courseId)
		{
			var args = new Dictionary<string, object>() { {Constants.CourseEventsPage.RequestParameters.CourseId, courseId } };
			var requestUri = BuildUriRequestString(Constants.CourseEventsPage.Uri, args);

			HttpResponseMessage response = await httpClient.GetAsync(requestUri);
			EnsureResponseCompletedSuccessfully(response);

			return CourseEvent.ListFromHttpContent(response.Content);
		}

		private string BuildUriRequestString(Uri uri, Dictionary<string,object> args)
		{
			var uriBuilder = new UriBuilder(uri);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			foreach(var keyValuePair in args)
			{
				query[keyValuePair.Key] = keyValuePair.Value.ToString();
			}
			uriBuilder.Query = query.ToString();
			return uriBuilder.ToString();
		}

		private void EnsureResponseCompletedSuccessfully(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					throw new InvalidCredentialsException("Login or password are invalid.");
				}
				throw new ServerErrorException("Unable to complere request.", response.StatusCode, response.Content, null);
			}
		}
	}
}

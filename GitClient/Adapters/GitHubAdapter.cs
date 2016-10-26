using GitClient.Models;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Issue = GitClient.Models.Issue;
using ItemState = Octokit.ItemState;
using ProductHeaderValue = Octokit.ProductHeaderValue;
using Repository = GitClient.Models.Repository;
using User = Octokit.User;

namespace GitClient.Adapters
{
	public class GitHubAdapter : IGitAdapter
	{
		private GitHubClient Client { get; }

		private User User { get; set; }
		private Login LoginInfo { get; }

		private List<Models.Repository> Repositories { get; }
		private List<Octokit.Repository> OctokitRepositories { get; }

		public GitHubAdapter(Login login, GitHubClient client = null)
		{
			if (client == null)
			{
				client = new GitHubClient(new ProductHeaderValue("GitClient"));
			}
			Client = client;
			LoginInfo = login;
			Repositories = new List<Repository>();
			OctokitRepositories = new List<Octokit.Repository>();
		}

		public async Task<bool> Login()
		{
			try
			{
				var basicAuth = new Credentials(LoginInfo.Username, LoginInfo.Password);
				Client.Credentials = basicAuth;

				User = await Client.User.Current();

				return User != null;
			}
			catch (AuthorizationException ex)
			{
				App.CurrentApp.Errors.Add(ex.Message);
				return false;
			}
			catch (ArgumentException ex)
			{
				App.CurrentApp.Errors.Add(ex.Message);
				return false;
			}
		}

		public async Task<Models.User> GetUserInfo()
		{
			if (User == null)
			{
				if (!await Login())
					return null;
			}

			if (User != null)

				return new Models.User()
				{
					Username = User.Login,
					AvatarUrl = User.AvatarUrl,
					Email = User.Email,
					Name = User.Name
				};
			return null;
		}

		public Login GetLoginInfo()
		{
			return LoginInfo;
		}

		public async Task LoadIssues()
		{
			foreach (var repository in Repositories)
			{
				var issues = await Client.Issue.GetAllForRepository(repository.Id);

				foreach (var issue in issues)
				{
					var newIssue = new Models.Issue()
					{
						Id = issue.Id,
						Body = issue.Body,
						CreatedAt = issue.CreatedAt,
						Number = issue.Number,
						State = (Models.ItemState)issue.State,
						Repository = new Models.Repository()
						{
							Id = repository.Id,
							FullName = repository.FullName,
							Name = repository.Name,
							HasIssues = repository.HasIssues,
							Language = repository.Language,
							OpenIssuesCount = repository.OpenIssuesCount
						},
						User = new Models.User()
						{
							Username = issue.User.Login,
							AvatarUrl = issue.User.AvatarUrl
						},
						Title = issue.Title,
						UpdatedAt = issue.UpdatedAt,
						CommentsUrl = issue.CommentsUrl,
						Url = issue.Url
					};
					if (issue.Comments > 0)
					{
						var comments = await Client.Issue.Comment.GetAllForIssue(repository.Id, issue.Number);
						var list = comments.Select(comment => new Comment()
						{
							Id = comment.Id,
							Body = comment.Body.Replace("strong", "Bold"),
							CreatedAt = comment.CreatedAt,
							User = new Models.User()
							{
								Username = comment.User.Login,
								AvatarUrl = comment.User.AvatarUrl
							}
						}).ToList();
						newIssue.Comments = list;
					}
					repository.Issues.Add(newIssue);
				}
			}
		}

		public IEnumerable<Models.Repository> GetRepositories()
		{
			return Repositories;
		}

		public async Task LoadRepositories()
		{
			Repositories.Clear();
			OctokitRepositories.Clear();

			var result = await Client.Repository.GetAllForCurrent();


			foreach (var repository in result)
			{
				OctokitRepositories.Add(repository);
				Repositories.Add(new Models.Repository()
				{
					Id = repository.Id,
					FullName = repository.FullName,
					Name = repository.Name,
					HasIssues = repository.HasIssues,
					Language = repository.Language,
					OpenIssuesCount = repository.OpenIssuesCount
				});
			}
		}

		private HttpClient CreateHttpClient()
		{
			var client = new HttpClient();
			var byteArray = Encoding.ASCII.GetBytes($"{LoginInfo.Username}:{LoginInfo.Password}");
			client.DefaultRequestHeaders.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
			client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "GitClient");

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			return client;
		}

		public async Task<bool> AddComment(Models.Issue issue, Comment comment)
		{
			try
			{
				using (var client = CreateHttpClient())
				{
					var json = JsonConvert.SerializeObject(new { body = comment.Body });
					var response = await client
						.PostAsync(issue.CommentsUrl,
							new StringContent(json, Encoding.UTF8, "application/json"));

					return response.IsSuccessStatusCode;
				}

			}
			catch (Exception)
			{
				return false;
			}

		}

		public async Task<bool> CloseIsse(Issue issue)
		{
			try
			{
				var repo = OctokitRepositories.FirstOrDefault(r => r.Id == issue.Repository.Id);
				if (repo == null)
					return false;

				var issueList = await Client.Issue.GetAllForRepository(repo.Id);

				var issueItem = issueList.FirstOrDefault(i => i.Id == issue.Id);

				if (issueItem == null)
					return false;

				var issueUpdate = issueItem.ToUpdate();

				issueUpdate.State = ItemState.Closed;
				

				var json = JsonConvert.SerializeObject(issueUpdate);

				var method = new HttpMethod("PATCH");

				var request = new HttpRequestMessage(method, issue.Url)
				{
					Content = new StringContent(json, Encoding.UTF8, "application/json")
				};

				var client = CreateHttpClient();

				var response = await client.SendAsync(request);

				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

	}
}

using GitClient.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
		public GitHubAdapter(Login login, GitHubClient client = null)
		{
			if (client == null)
			{
				client = new GitHubClient(new ProductHeaderValue("GitClient"));
			}
			Client = client;
			LoginInfo = login;
			Repositories = new List<Repository>();
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
						User = new Models.User()
						{
							Username = issue.User.Login,
							AvatarUrl = issue.User.AvatarUrl
						},
						Title = issue.Title,
						UpdatedAt = issue.UpdatedAt
					};
					if (issue.Comments > 0)
					{
						var comments = await Client.Issue.Comment.GetAllForIssue(repository.Id, issue.Number);
						var list = comments.Select(comment => new Comment()
						{
							Id = comment.Id,
							Body = comment.Body,
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

			var result = await Client.Repository.GetAllForCurrent();

			foreach (var repository in result)
			{

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
	}
}

using GitClient.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository = GitClient.Models.Repository;
using User = Octokit.User;

namespace GitClient.Adapters
{
	public class GitHubAdapter : IGitAdapter
	{
		private GitHubClient Client { get; }

		private User User { get; set; }
		public Login LoginInfo { get; }

		public List<Models.Repository> Repositories { get; set; }
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

		public Models.User GetUserInfo()
		{
			return new Models.User()
			{
				Username = User.Login,
				Email = User.Email,
				Name = User.Name
			};
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
					repository.Issues.Add(new Models.Issue()
					{
						Title = issue.Title,
						Body = issue.Body,
						User = new Models.User()
						{
							Username = issue.User.Login
						}
					});
				}
			}
		}

		public IEnumerable<Models.Repository> GetRepositories()
		{
			return Repositories;
		}

		public async Task LoadRepositories()
		{
			var result = await Client.Repository.GetAllForCurrent();

			foreach (var repository in result)
			{
				Repositories.Add(new Models.Repository()
				{
					Id = repository.Id,
					FullName = repository.FullName,
					Name = repository.Name,
					HasIssues = repository.HasIssues
				});
			}
		}
	}
}

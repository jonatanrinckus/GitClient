using Octokit;
using System;
using System.Threading.Tasks;

namespace GitClient.Adapters
{
	public class GitHubAdapter : IGitAdapter
	{
		private GitHubClient Client { get; }

		private User User { get; set; }

		public GitHubAdapter(GitHubClient client = null)
		{
			if (client == null)
			{
				client = new GitHubClient(new ProductHeaderValue("GitClient"));
			}
			Client = client;
		}

		public async Task<bool> Login(string login, string password)
		{
			try
			{
				var basicAuth = new Credentials(login, password);
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
				Email = User.Email,
				Name = User.Name
			};
		}
	}
}

using GitClient.Adapters;
using GitClient.Models;
using System;

namespace GitClient.Factories
{
	public static class LoginFactory
	{
		public static IGitAdapter CreateInstance(Login login)
		{
			if (login == null)
				throw new ArgumentNullException(nameof(login), "Login is null");

			switch (login.Provider)
			{
				case Provider.GitHub:
					return new GitHubAdapter(login);
				case Provider.GitLab:
				default:
					return null;
			}

		}
	}
}

﻿using GitClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitClient.Adapters
{
	public interface IGitAdapter
	{
		Task<bool> Login();
		User GetUserInfo();
		Login GetLoginInfo();
		Task LoadIssues();
		IEnumerable<Repository> GetRepositories();
		Task LoadRepositories();
	}
}

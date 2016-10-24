using GitClient.Adapters;
using GitClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitClient.Composites
{
	public class GitComposite : IGitAdapter
	{
		public List<IGitAdapter> GitAdapters { get; }

		public IGitAdapter InUse { get; set; }

		public GitComposite()
		{
			GitAdapters = new List<IGitAdapter>();
		}


		public IGitAdapter GetAdapter(Login login)
		{
			return GitAdapters.FirstOrDefault(a => a.GetLoginInfo() == login);
		}

		public bool Add(IGitAdapter adapter)
		{
			if (adapter == null)
				return false;

			if (
				GitAdapters.Any(
					g =>
						g.GetLoginInfo().Username == adapter.GetLoginInfo().Username &&
						g.GetLoginInfo().Provider == adapter.GetLoginInfo().Provider))
				return true;

			GitAdapters.Add(adapter);
			return true;

		}

		public bool Remove(int position)
		{
			if (position < 0)
				return false;

			if (GitAdapters.Count < position + 1)
				return false;

			GitAdapters.RemoveAt(position);
			return true;
		}

		public bool Remove(IGitAdapter adapter)
		{
			if (adapter == null)
				return false;

			if (!GitAdapters.Contains(adapter))
				return false;

			GitAdapters.Remove(adapter);
			return true;
		}

		public async Task<IEnumerable<User>> GetUsers()
		{
			var list = new List<User>();
			foreach (var adapter in GitAdapters)
			{
				list.Add(await adapter.GetUserInfo());
			}

			return list;
		}

		public async Task<bool> Login()
		{
			if (!GitAdapters.Any())
				return false;

			foreach (var adapter in GitAdapters)
			{
				if (await adapter.Login())
					continue;

				return false;
			}
			return true;
		}

		public async Task<User> GetUserInfo()
		{
			return await InUse.GetUserInfo();
		}

		public Login GetLoginInfo()
		{
			return InUse.GetLoginInfo();
		}

		public async Task LoadIssues()
		{
			foreach (var adapter in GitAdapters)
			{
				await adapter.LoadIssues();
			}
		}


		public IEnumerable<Repository> GetRepositories()
		{
			return InUse.GetRepositories();
		}

		public async Task LoadRepositories()
		{
			foreach (var adapter in GitAdapters)
			{
				await adapter.LoadRepositories();
			}
		}

		public async Task<bool> AddComment(Issue issue, Comment comment)
		{
			return await InUse.AddComment(issue, comment);
		}
	}
}

using GitClient.Adapters;
using GitClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitClient.Composites
{
	public class GitComposite : IGitAdapter
	{
		private IGitAdapter _inUse;
		public List<IGitAdapter> GitAdapters { get; }

		public IGitAdapter InUse
		{
			get { return _inUse; }
			set
			{
				if (!GitAdapters.Contains(value))
					GitAdapters.Add(value);

				_inUse = value;
			}
		}

		public GitComposite()
		{
			GitAdapters = new List<IGitAdapter>();
		}


		public bool Add(IGitAdapter adapter)
		{
			if (adapter == null)
				return false;

			if (GitAdapters.Contains(adapter))
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

		public User GetUserInfo()
		{
			return InUse.GetUserInfo();
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


	}
}

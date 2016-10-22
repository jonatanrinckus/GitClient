using GitClient.Models;
using System.Collections.ObjectModel;

namespace GitClient.ViewModels
{
	public class IssueViewModel : BaseViewModel
	{
		private ObservableCollection<Repository> _repositories;

		public ObservableCollection<Repository> Repositories
		{
			get { return _repositories; }
			set { SetProperty(ref _repositories, value); }
		}

		private ObservableCollection<Login> _logins;

		public ObservableCollection<Login> Logins
		{
			get { return _logins; }
			set { SetProperty(ref _logins, value); }
		}

		private ObservableCollection<User> _users;

		public ObservableCollection<User> Users
		{
			get { return _users; }
			set { SetProperty(ref _users, value); }
		}






	}
}

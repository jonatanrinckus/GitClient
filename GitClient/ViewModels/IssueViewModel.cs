using GitClient.Models;
using System.Collections.ObjectModel;

namespace GitClient.ViewModels
{
	public class IssueViewModel : BaseViewModel
	{
		private ObservableCollection<User> _users;
		private ObservableCollection<Repository> _repositories;
		private bool _isLoading;

		public IssueViewModel()
		{
			Users = new ObservableCollection<User>();
			Repositories = new ObservableCollection<Repository>();
		}

		public ObservableCollection<User> Users
		{
			get { return _users; }
			set { SetProperty(ref _users, value); }
		}


		public bool IsLoading
		{
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}



		public ObservableCollection<Repository> Repositories
		{
			get { return _repositories; }
			set { SetProperty(ref _repositories, value); }
		}


		private User _lastUserSelected;

		public User LastUserSelected
		{
			get { return _lastUserSelected; }
			set { SetProperty(ref _lastUserSelected, value); }
		}


	}
}

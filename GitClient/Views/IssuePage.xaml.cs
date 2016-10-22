using GitClient.Models;
using GitClient.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GitClient.Views
{
	/// <summary>
	/// Interaction logic for IssuePage.xaml
	/// </summary>
	public partial class IssuePage : Page
	{
		private MainViewModel MainContext { get; }
		private IssueViewModel Context { get; }
		private ObservableCollection<User> Users = new ObservableCollection<User>();
		private ObservableCollection<Repository> Repositories = new ObservableCollection<Repository>();
		private ObservableCollection<Issue> Issues = new ObservableCollection<Issue>();
		private bool IsLoading;


		public IssuePage(MainViewModel mainMainContext)
		{
			MainContext = mainMainContext;
			InitializeComponent();
			Context = (IssueViewModel)DataContext;
			AccountsComboBox.ItemsSource = Users;
			RepositoriesListView.ItemsSource = Repositories;
			IssuesListView.ItemsSource = Issues;

			Load();
		}

		private async void Load()
		{
			if (IsLoading)
				return;

			IsLoading = true;
			var inUseAdapter = App.AppManager.Composite.InUse;
			MainContext.Status = $"Loading repositorios...";
			await inUseAdapter.LoadRepositories().ContinueWith(t =>
			{
				if (t.IsCompleted)
				{
					MainContext.Status = $"Loading issues...";
					inUseAdapter.LoadIssues().ContinueWith(t2 =>
					{
						MainContext.Status = t2.IsCompleted
							? $"Repositories and issues loaded successfully"
							: $"Error to load issues";
					}).ContinueWith(task =>
					{
						Task.Factory.StartNew(() =>
						{
							Task.Delay(3000).ContinueWith(t3 =>
							{
								MainWindow.SetStatus();
							});
						});
					});
				}
				else
					MainContext.Status = $"Error to load repositories";

			});


			Inicialize();

			IsLoading = false;



		}

		private async void Inicialize()
		{
			Clear();

			var users = await App.AppManager.Composite.GetUsers();
			foreach (var user in users)
			{
				Users.Add(user);
			}

			var repos = App.AppManager.Composite.InUse.GetRepositories();

			foreach (var repository in repos)
			{
				Repositories.Add(repository);
			}

			AccountsComboBox.SelectedIndex = App.AppManager.Logins.IndexOf(App.AppManager.Composite.InUse.GetLoginInfo());
		}

		private void OnSelectionAccountsComboBoxChanged(object sender, SelectionChangedEventArgs e)
		{

			if (IsLoading)
				return;

			if (e.AddedItems.Count <= 0)
				return;

			var user = (User)e.AddedItems[0];

			var login = App.AppManager.Logins.FirstOrDefault(l => l.Username == user.Username && l.Provider == user.Provider);

			App.AppManager.Composite.InUse = App.AppManager.Composite.GetAdapter(login);

			Load();
		}

		private void OnRepositoriesListViewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Issues.Clear();
			var item = (Repository)((ListBox)sender).SelectedItem;

			foreach (var issue in item.Issues)
			{
				Issues.Add(issue);
			}

		}

		public void Clear()
		{
			Users.Clear();
			Repositories.Clear();
			Issues.Clear();
		}
	}
}

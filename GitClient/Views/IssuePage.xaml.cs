using GitClient.Models;
using GitClient.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace GitClient.Views
{
	/// <summary>
	/// Interaction logic for IssuePage.xaml
	/// </summary>
	public partial class IssuePage : Page
	{
		private MainViewModel MainContext { get; }
		private IssueViewModel Context { get; }
		public IssuePage(MainViewModel mainMainContext)
		{
			MainContext = mainMainContext;
			InitializeComponent();
			Context = (IssueViewModel)DataContext;

			Load();

			Context.Logins = new ObservableCollection<Login>(App.AppManager.Logins);

			Context.Users = new ObservableCollection<User>(App.AppManager.Composite.GetUsers().Result);

			AccountsComboBox.SelectedIndex = Context.Logins.IndexOf(App.AppManager.Composite.InUse.GetLoginInfo());
		}

		private async void Load()
		{
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
					});
				}
				else
					MainContext.Status = $"Error to load repositories";

			});


		}
	}
}

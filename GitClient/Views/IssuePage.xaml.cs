﻿using GitClient.Models;
using GitClient.ViewModels;
using System.Linq;
using System.Threading;
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


		public IssuePage(MainViewModel mainMainContext)
		{
			MainContext = mainMainContext;
			InitializeComponent();
			Context = (IssueViewModel)DataContext;
			Load();
		}

		public async void Load()
		{
			if (Context.IsLoading)
				return;

			Context.IsLoading = true;
			var inUseAdapter = App.AppManager.Composite.InUse;
			MainContext.Status = "Loading repositorios...";
			await inUseAdapter.LoadRepositories().ContinueWith(t =>
			{
				var t4 = new Thread(() =>
				{
					if (t.IsCompleted)
					{

						MainContext.Status = "Loading issues...";
						inUseAdapter.LoadIssues().ContinueWith(t2 =>
						{
							MainContext.Status = t2.IsCompleted
							? "Repositories and issues loaded successfully"
							: "Error to load issues";


						}).ContinueWith(task =>
						{
							Task.Factory.StartNew(() =>
							{

								Context.IsLoading = false;
								Task.Delay(3000).ContinueWith(t3 =>
								{
									MainWindow.SetStatus();
								});
							});
						});
					}
					else
						MainContext.Status = "Error to load repositories";
				});

				t4.Start();
				

			});


			Inicialize();


		}

		private async void Inicialize()
		{
			Context.Users.Clear();
			Context.Repositories.Clear();

			var users = await App.AppManager.Composite.GetUsers();
			foreach (var user in users)
			{
				Context.Users.Add(user);
			}

			var repos = App.AppManager.Composite.InUse.GetRepositories();

			foreach (var repository in repos)
			{
				Context.Repositories.Add(repository);
			}

			AccountsComboBox.SelectedIndex = App.AppManager.Logins.IndexOf(App.AppManager.Composite.InUse.GetLoginInfo());

			Context.UserSelected = (User)AccountsComboBox.SelectedItem;

		}

		private void OnSelectionAccountsComboBoxChanged(object sender, SelectionChangedEventArgs e)
		{

			if (Context.IsLoading)
				return;

			if (e.AddedItems.Count <= 0)
				return;

			var user = (User)e.AddedItems[0];

			if (Context.UserSelected?.Provider == user?.Provider &&
				Context.UserSelected?.Username == user?.Username)
				return;

			var login = App.AppManager.Logins.FirstOrDefault(l => l.Username == user?.Username && l.Provider == user?.Provider);

			App.AppManager.Composite.InUse = App.AppManager.Composite.GetAdapter(login);

			Load();

			if (IssueFrame.NavigationService.CanGoBack)
				IssueFrame.NavigationService.GoBack();
		}


		private void Clear()
		{
			Context.Users.Clear();
			Context.Repositories.Clear();
		}


		private void OnRepositoriesListViewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (IssueFrame.NavigationService.CanGoBack)
				IssueFrame.NavigationService.GoBack();
		}


		private void OnIssueMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var item = (Issue)IssuesListView.SelectedItem;

			IssueFrame.NavigationService.Navigate(new IssueDetailPage(item, this));

		}
	}
}

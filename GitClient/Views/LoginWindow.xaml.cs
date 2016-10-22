using GitClient.Factories;
using GitClient.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GitClient.Views
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{

		public LoginWindow()
		{
			InitializeComponent();

			ProviderComboBox.ItemsSource = Enum.GetValues(typeof(Provider));

			LoginsListView.ItemsSource = App.AppManager.Logins;
		}

		private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
		{
			var login = DataContext as Login;

			if (login == null)
				return;

			login.Password = PasswordBox.Password;

			await Login(login);
		}

		private async Task Login(Login login)
		{
			try
			{
				var adapter = LoginFactory.CreateInstance(login);

				App.AppManager.Composite.InUse = adapter;

				if (login == null)
					throw new NullReferenceException("Login is null.");

				if (await adapter.Login())
				{

					if (!App.AppManager.Contains(login))
						App.AppManager.AddLogin(login);

					DialogResult = true;
					Close();
				}
				else
				{
					if (App.CurrentApp.HasError)
					{
						while (App.CurrentApp.Errors.Any())
						{
							var error = App.CurrentApp.Errors.FirstOrDefault();
							MessageBox.Show(error, "Error", MessageBoxButton.OK);
							App.CurrentApp.Errors.Remove(error);
						}
					}
					else
					{
						MessageBox.Show("A error ocurred, please try again in a few seconds " +
										"or contact the support", "Error", MessageBoxButton.OK);
					}
				}
			}
			catch (NullReferenceException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
			}
		}


		private void OnCancelButtonClick(object sender, RoutedEventArgs e)
		{
			Close();
		}


		private void OnLoginsListViewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			var item = LoginsListView.SelectedItem as Login;

			if (item == null)
				return;

			var result = MessageBox.Show($"Would you like to delete the login for {item.Provider} " +
							$"with the username {item.Username}?",
							$"Delete login for {item.Provider}",
							MessageBoxButton.YesNo);

			if (result != MessageBoxResult.Yes)
				return;

			App.AppManager.RemoveLogin(item);

			LoginsListView.ItemsSource = App.AppManager.Logins;

			LoginsListView.Items.Refresh();
		}

		private async void OnLoginsListViewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var item = LoginsListView.SelectedItem as Login;

			if (item == null)
				return;

			await Login(item);

		}
	}
}

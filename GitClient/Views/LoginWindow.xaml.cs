using GitClient.Adapters;
using GitClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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

			ProviderComboBox.ItemsSource = new List<string>()
			{
				"GitHub"
			};

		}

		private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
		{
			try
			{
				IGitAdapter client = new GitHubAdapter();
				var login = DataContext as Login;

				if(login == null)
					throw new NullReferenceException("Login is null.");

				login.Password = PasswordBox.Password;

				if (await client.Login(login))
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
	}
}
